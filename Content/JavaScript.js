$(document).ready(function () {

    var ch = false;
    $('.Quiz').on('change', ".variant", function () {
        $(this).parents(".options").find(".variant").not(this).prop("checked", false)
    });

    $(".tbody").on("click", "#Remove", function () {

        alert();

        id = $(this).parents("tr").children(".user-id").text();
        var a = document.getElementById('deleteput'); //or grab it by tagname etc
        a.href = "/Admin/Delete/" + id;
    })
    $("#Remove").click(function () {

        alert();
    })
})