document.getElementById('Registration').style.display='none';

function Register(){
    document.getElementById('Registration').style.display='flex';
    document.getElementById('Log_In').style.display='none';
}
function Account(){
    document.getElementById('Log_In').style.display='flex';
    document.getElementById('Registration').style.display='none';
}
function pass_eye(){
    var toggle_eye = document.getElementById("toggle_eye_log")
    var password = document.getElementById("pass_in")
    var type = password.getAttribute("type") === "password" ? "text" : "password";
    password.setAttribute("type", type);
    // toggle_eye.classList.replace("far fa-eye-slash", "far fa-eye");
    if (toggle_eye.classList.contains("fa-eye")){
        toggle_eye.classList.remove("fa-eye");
        toggle_eye.classList.add("fa-eye-slash");
    }else{
        toggle_eye.classList.remove("fa-eye-slash");
        toggle_eye.classList.add("fa-eye");
    }

}
function sing_pass_eye(){
    var toggle_eye = document.getElementById("toggle_eye_sign")
    var password = document.getElementById("sign_pass")
    var type = password.getAttribute("type") === "password" ? "text" : "password";
    password.setAttribute("type", type);
    // toggle_eye.classList.replace("far fa-eye-slash", "far fa-eye");
    if (toggle_eye.classList.contains("fa-eye")){
        toggle_eye.classList.remove("fa-eye");
        toggle_eye.classList.add("fa-eye-slash");
    }else{
        toggle_eye.classList.remove("fa-eye-slash");
        toggle_eye.classList.add("fa-eye");
    }

}
function re_pass_eye(){
    var toggle_eye = document.getElementById("toggle_eye_re")
    var password = document.getElementById("re_pass")
    var type = password.getAttribute("type") === "password" ? "text" : "password";
    password.setAttribute("type", type);
    // toggle_eye.classList.replace("far fa-eye-slash", "far fa-eye");
    if (toggle_eye.classList.contains("fa-eye")){
        toggle_eye.classList.remove("fa-eye");
        toggle_eye.classList.add("fa-eye-slash");
    }else{
        toggle_eye.classList.remove("fa-eye-slash");
        toggle_eye.classList.add("fa-eye");
    }

}