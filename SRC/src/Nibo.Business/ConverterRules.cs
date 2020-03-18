using Nibo.Business.Enum;
using Nibo.Business.Interfaces;
using Nibo.Business.Models;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Nibo.Business
{
    public class ConverterRules : IConverterRules
    {
        public BankStatement GenerateBankStatement(string ofxFile)
        {
            return GetBankStatement(ofxFile);
        }

        public static BankStatement GetBankStatement(string ofxFile)
        {
            bool hasHeader = false;
            bool hasAccountData = false;

            ExportToXml(ofxFile, ofxFile + ".xml");
            string elementRead = "";
            BankTransactions currentTransactions = null;

            BankStatementHeader header = new BankStatementHeader();
            BankAccount account = new BankAccount();
            BankStatement statement = new BankStatement(header, account);
            statement.BankAccountId = account.Id;

            XmlTextReader meuXml = new XmlTextReader(ofxFile + ".xml");
            try
            {
                while (meuXml.Read())
                {
                    if (meuXml.NodeType == XmlNodeType.EndElement)
                    {
                        switch (meuXml.Name)
                        {
                            case "STMTTRN":
                                if (currentTransactions != null)
                                {
                                    statement.AddTransaction(currentTransactions);
                                    currentTransactions = null;
                                }
                                break;
                        }
                    }
                    if (meuXml.NodeType == XmlNodeType.Element)
                    {
                        elementRead = meuXml.Name;

                        switch (elementRead)
                        {
                            case "STMTTRN":
                                currentTransactions = new BankTransactions();
                                break;
                        }
                    }
                    if (meuXml.NodeType == XmlNodeType.Text)
                    {
                        switch (elementRead)
                        {
                            case "DTSERVER":
                                header.ServerDate = ConvertOfxDateToDateTime(meuXml.Value, statement);
                                hasHeader = true;
                                break;
                            case "LANGUAGE":
                                header.Language = meuXml.Value;
                                hasHeader = true;
                                break;
                            case "DTSTART":
                                statement.InitialDate = ConvertOfxDateToDateTime(meuXml.Value, statement);
                                break;
                            case "DTEND":
                                DateTime dtEnd = ConvertOfxDateToDateTime(meuXml.Value, statement);
                                if (dtEnd != DateTime.MinValue)
                                    statement.FinalDate = dtEnd;
                                break;
                            case "BANKID":
                                account.Bank = new Bank(GetBankId(meuXml.Value, statement), GetBankName(meuXml.Value));
                                account.BankId = account.Bank.Id;
                                hasAccountData = true;
                                break;
                            case "BRANCHID":
                                account.AgencyCode = meuXml.Value;
                                hasAccountData = true;
                                break;
                            case "ACCTID":
                                account.AccountCode = meuXml.Value;
                                hasAccountData = true;
                                break;
                            case "ACCTTYPE":
                                account.Type = meuXml.Value;
                                hasAccountData = true;
                                break;
                            case "TRNTYPE":
                                currentTransactions.Type = meuXml.Value;
                                break;
                            case "DTPOSTED":
                                currentTransactions.Date = ConvertOfxDateToDateTime(meuXml.Value, statement);
                                break;
                            case "TRNAMT":
                                currentTransactions.TransactionValue = GetTransactionValue(meuXml.Value, statement);
                                break;
                            case "MEMO":
                                currentTransactions.Description = string.IsNullOrEmpty(meuXml.Value) ? "" : meuXml.Value.Trim().Replace("  ", " ");
                                break;
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                throw new Exception("Invalid OFX file!" + ex);
            }
            finally
            {
                meuXml.Close();
            }

            if ((!hasHeader) || (!hasAccountData))
            {
                throw new Exception("Invalid OFX file!");
            }

            return statement;
        }

        private static int GetBankId(string value, BankStatement statement)
        {
            int bankId;
            if (!int.TryParse(value, out bankId))
                bankId = 0;

            return bankId;
        }

        private static string GetBankName(string value)
        {
            string bankName = string.Empty;
            switch (value)
            {
                case "001":
                    bankName = "Banco do Brasil S.A.";
                    break;
                case "033":
                    bankName = "Banco Santander(Brasil) S.A.";
                    break;
                case "104":
                    bankName = "Caixa Econômica Federal";
                    break;
                case "237":
                    bankName = "Banco Bradesco S.A.";
                    break;
                case "0341":
                    bankName = "Banco Itaú S.A.";
                    break;
                case "389":
                    bankName = "Banco Mercantil do Brasil S.A.";
                    break;
                case "399":
                    bankName = "HSBC Bank Brasil S.A. – Banco Múltiplo";
                    break;
                case "422":
                    bankName = "Banco Safra S.A.";
                    break;
                case "453":
                    bankName = "Banco Rural S.A.";
                    break;
                case "745":
                    bankName = "Banco Citibank S.A.";
                    break;
            }
            return bankName;
        }

        private static void ExportToXml(String ofxSourceFile, String xmlNewFile)
        {
            if (File.Exists(ofxSourceFile))
            {
                if (xmlNewFile.ToLower().EndsWith(".xml"))
                {
                    StringBuilder ofxTranslated = TranslateToXml(ofxSourceFile);

                    if (File.Exists(xmlNewFile))
                        File.Delete(xmlNewFile);

                    StreamWriter sw = File.CreateText(xmlNewFile);
                    sw.WriteLine(@"<?xml version=""1.0""?>");
                    sw.WriteLine(ofxTranslated.ToString());
                    sw.Close();
                }
                else
                {
                    throw new ArgumentException("Name of new XML file is not valid: " + xmlNewFile);
                }
            }
            else
            {
                throw new FileNotFoundException("OFX source file not found: " + ofxSourceFile);
            }
        }

        private static StringBuilder TranslateToXml(string pathToOfxFile)
        {
            StringBuilder resultado = new StringBuilder();
            int nivel = 0;
            string linha;

            if (!File.Exists(pathToOfxFile))
            {
                throw new FileNotFoundException("OFX source file not found: " + pathToOfxFile);
            }

            StreamReader sr = File.OpenText(pathToOfxFile);
            while ((linha = sr.ReadLine()) != null)
            {
                linha = linha.Trim();

                if (linha.StartsWith("</") && linha.EndsWith(">"))
                {
                    AddTabs(resultado, nivel, true);
                    nivel--;
                    resultado.Append(linha);
                }
                else if (linha.StartsWith("<") && linha.EndsWith(">"))
                {
                    nivel++;
                    AddTabs(resultado, nivel, true);
                    resultado.Append(linha);
                }
                else if (linha.StartsWith("<") && !linha.EndsWith(">"))
                {
                    AddTabs(resultado, nivel + 1, true);
                    resultado.Append(linha);
                    resultado.Append(ReturnFinalTag(linha));
                }
            }
            sr.Close();

            return resultado;
        }

        private static void AddTabs(StringBuilder stringObject, int lengthTabs, bool newLine)
        {
            if (newLine)
            {
                stringObject.AppendLine();
            }
            for (int j = 1; j < lengthTabs; j++)
            {
                stringObject.Append("\t");
            }
        }

        private static string ReturnFinalTag(String content)
        {
            string returnFinal = "";

            if ((content.IndexOf("<") != -1) && (content.IndexOf(">") != -1))
            {
                int position1 = content.IndexOf("<");
                int position2 = content.IndexOf(">");
                if ((position2 - position1) > 2)
                {
                    returnFinal = content.Substring(position1, (position2 - position1) + 1);
                    returnFinal = returnFinal.Replace("<", "</");
                }
            }

            return returnFinal;
        }

        private static DateTime ConvertOfxDateToDateTime(string ofxDate, BankStatement statement)
        {
            DateTime dateTimeReturned = DateTime.MinValue;
            try
            {
                int year = GetPartOfOfxDate(ofxDate, Enumerators.PartDateTime.YEAR);
                int month = GetPartOfOfxDate(ofxDate, Enumerators.PartDateTime.MONTH);
                int day = GetPartOfOfxDate(ofxDate, Enumerators.PartDateTime.DAY);
                int hour = GetPartOfOfxDate(ofxDate, Enumerators.PartDateTime.HOUR);
                int minute = GetPartOfOfxDate(ofxDate, Enumerators.PartDateTime.MINUTE);
                int second = GetPartOfOfxDate(ofxDate, Enumerators.PartDateTime.SECOND);

                dateTimeReturned = new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception)
            { }

            return dateTimeReturned;
        }

        private static int GetPartOfOfxDate(string ofxDate, Enumerators.PartDateTime partDateTime)
        {
            int result = 0;

            switch (partDateTime)
            {
                case Enumerators.PartDateTime.DAY:
                    result = int.Parse(ofxDate.Substring(6, 2));
                    break;
                case Enumerators.PartDateTime.MONTH:
                    result = int.Parse(ofxDate.Substring(4, 2));
                    break;
                case Enumerators.PartDateTime.YEAR:
                    result = int.Parse(ofxDate.Substring(0, 4));
                    break;
                case Enumerators.PartDateTime.HOUR:
                    if (ofxDate.Length >= 10)
                        result = int.Parse(ofxDate.Substring(8, 2));
                    break;
                case Enumerators.PartDateTime.MINUTE:
                    if (ofxDate.Length >= 12)
                        result = int.Parse(ofxDate.Substring(10, 2));
                    break;
                case Enumerators.PartDateTime.SECOND:
                    if (ofxDate.Length >= 14)
                        result = int.Parse(ofxDate.Substring(12, 2));
                    break;
            }

            return result;
        }

        private static decimal GetTransactionValue(string value, BankStatement statement)
        {
            return Convert.ToDecimal(value.Replace('.', ','));
        }

    }


}
