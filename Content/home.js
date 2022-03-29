$(document).ready(function(){

    
    
    $("#search_in").hide();
    $("#cancel").hide();
    
    $("#search_icon").click(function(){
        $("#search_in").show();
        $("#cancel").show();
        $("#search").width("20%")
    });
    
    $("#cancel").click(function(){
        $("#search_in").hide();
        $("#cancel").hide();
        $("#search").width("2%")
    });
    
    $("#Add").click(function(){
        $("#Add-up").show();
        var model = document.getElementById("Add-up")
        window.onclick=function(light){
            if(light.target == model){
                model.style.display = "none";
            }
        };
    });
    
    $("#Alert").hide();
    
    $("#Add-cancel").click(function(){
        $("#Add-up").hide();
    });
    $("#Remove").click(function(){
        $("#Alert").show()
        $("#Alert-n").click(function(){
            $("#Alert").hide();
        });
        $("#Alert-y").click(function(){
            $("#Alert").hide();
        
        });
        
        var modal = document.getElementById("Alert")
        window.onclick=function(event){
            if(event.target == modal){
                modal.style.display = "none";
            }
        };
    });

    $(".tbody").on("click", "#Remove", function () {

        id = $(this).parents("tr").children(".user-id").text();
        var a = document.getElementById('deleteput'); //or grab it by tagname etc
        a.href = "/Admin/Delete/" + id;

        $("#Alert").show();
        $("#Alert-n").click(function () {
            $("#Alert").hide();
        });
        $("#Alert-y").click(function () {
            $("#Alert").hide();

        });

        var modal = document.getElementById("Alert")
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        };
    })
    $("#Edit-pg").click(function(){
        window.location.href="../edit page/edit page.html"
    });

    $("#Ed-cancel").click(function(){
        window.location.href="../../home page/home.html"
    });
    var modal = document.getElementById("Edit")
    window.onclick=function(event){
        if(event.target == modal){
            window.location.href="../../home page/home.html"
        }
    };
    });