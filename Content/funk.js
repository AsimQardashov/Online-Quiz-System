$(document).ready(function () {

    $("#search_in").hide();
    $("#cancel").hide();

    $("#search_button").click(function () {
        $("#search_in").show();
        $("#search_in").find("input").focus();
        $("#cancel").show();
        $("#search").width("90%")
        var modal = document.getElementById("search")
        window.onclick = function (dart) {
            if (dart.target == modal) {
                $("#search_in").hide();
                $("#cancel").hide();
                $("#search").width("24px")
            }
        };
        // $("Search_in").blur(function(){

        //     ("#search_in").hide();
        //     $("#cancel").hide();
        //     $("#search").width("2%")
        // });
    });
    $(".search-tr").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $("#cancel").click(function () {
        $("#search_in").hide();
        $("#cancel").hide();
        $("#search").width("24px")
    });

    $("#Add").click(function () {
        $("#Add-up").show();
        var model = document.getElementById("Add-up")
        window.onclick = function (light) {
            if (light.target == model) {
                model.style.display = "none";
            }
        };
    });

    $("#Alert").hide();

    $("#Add-cancel").click(function () {
        $("#Add-up").hide();
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

    $(".tbody").on("click", "#RemoveT", function () {

        idT = $(this).parents("tr").children(".user-id").text();
        var a = document.getElementById('deleteput'); //or grab it by tagname etc
        a.href = "/Admin/DeleteT/" + idT;

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
    //$(".Admin-Remove").on('click', function () {

    //    id = $(this).parents("tr").children(".user-id").text();
    //    var a = document.getElementById('deleteput'); //or grab it by tagname etc
    //    a.href = "/Admin/Delete/" + id;

    //    $("#Alert").show()
    //    $("#Alert-n").click(function(){
    //        $("#Alert").hide();
    //    });
    //    $("#Alert-y").click(function(){
    //        $("#Alert").hide();

    //    });

    //// $("#Admin-Remove").click(function(){
    ////     $("#Alert").show()
    ////     $("#Alert-n").click(function(){
    ////         $("#Alert").hide();
    ////     });
    ////     $("#Alert-y").click(function(){
    ////         $("#Alert").hide();

    ////     });

    //    var modal = document.getElementById("Alert")
    //    window.onclick=function(event){
    //        if(event.target == modal){
    //            modal.style.display = "none";
    //        }
    //    };
    //});
    $(".Edit-pg").click(function () {
        window.location.href = "../edit page/edit page.html"
    });

    $("#Ed-cancel").click(function () {
        parent.history.back();
        return false;
    });
    var modal = document.getElementById("Edit")
    window.onclick = function (event) {
        if (event.target == modal) {
            // window.location.href="../../home page/home.html"
            parent.history.back();
            return false;
        }
    };


    //profile
    var profile_tesdiq = $(".profile-tesdiq-et").hide();
    var profile_legv = $(".profile-ləğv-et").hide();
    $(".profile-edit").click(function () {
        profile_tesdiq.show();
        profile_legv.show();
        $.each($(".profile-card-input"), function () {
            var t = $(this).text();
            $(this).css("display", "none")
            $(this).parent().children("input").val(t).css("display", "block");
        })
        $(this).hide();
        profile_tesdiq.click(function () {
            profile_tesdiq.hide();
            profile_legv.hide();
            $.each($(".profile-card-input"), function () {
                $(this).parent().children("input").css("display", "none");
                var y = $(this).parent().children("input").val();
                $(this).text(y).css("display", "block")
            })
            $(".profile-edit").show();
        });
        profile_legv.click(function () {
            profile_tesdiq.hide();
            profile_legv.hide();
            $(".profile-edit").show();
            $.each($(".profile-card-input"), function () {
                $(this).parent().children("input").css("display", "none");
                $(this).css("display", "block")
            })
        });
    })
});