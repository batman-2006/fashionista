﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function myFunction() {
    var x = document.getElementById("print");
    x.style.display = "block";
    var y = document.getElementById("btn");

    y.style.display = "none";
    setTimeout(() => {
        window.print();

    }
    
    
        , 2000);

}