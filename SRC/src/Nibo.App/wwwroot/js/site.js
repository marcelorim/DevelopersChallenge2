// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    var fileInput = document.getElementById('files');
    var listFile = document.getElementById('list_file');
    var fileInputReset = document.getElementById('btnReset');

    fileInput.onchange = function () {
        var files = Array.from(this.files);
        files = files.map(file => file.name);
        listFile.innerHTML = files.join(',&nbsp;');
    };

    fileInputReset.onclick = function () {
        this.files = null;
        listFile.innerHTML = "No file selected.";
    };
});