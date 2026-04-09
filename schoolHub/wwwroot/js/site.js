// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const show_hide_LoginPassword = document.getElementById("show-hide_LoginPassword");
const LoginPassword = document.getElementById("LoginPassword");
show_hide_LoginPassword.addEventListener("click", show_hide_LoginPassword_click);

const show_hide_RegisterPassword = document.getElementById("show-hide_RegisterPassword");
const RegisterPassword = document.getElementById("RegisterPassword");
show_hide_RegisterPassword.addEventListener("click", show_hide_RegisterPassword_click);


function show_hide_LoginPassword_click()
{
    switch (show_hide_LoginPassword.innerText)
    {
        case "show":
            LoginPassword.setAttribute("type","button");
            show_hide_LoginPassword.innerText = "hide";
            break;
        case "hide":
            LoginPassword.setAttribute("type","password");
            show_hide_LoginPassword.innerText = "show";
            break;
    }
}

function show_hide_RegisterPassword_click()
{
    switch (show_hide_RegisterPassword.innerText)
    {
        case "show":
            RegisterPassword.setAttribute("type", "button");
            show_hide_RegisterPassword.innerText = "hide";
            break;
        case "hide":
            RegisterPassword.setAttribute("type", "password");
            show_hide_RegisterPassword.innerText = "show";
            break;
    }
}