$(document).ready(function () {
    $("#login-btn").click(function () {
        var roleMap = {
            "学生": 1,
            "教师": 2,
            "教务": 3,
            "管理员": 4
        };
        if ($("#userid-input").val() == "" || $("#passwd-input").val() == "") {
            $("#alert1").hide();
            $("#alert2").show();
        }
        else {
            var data = {
                userId: $("#userid-input").val(),
                passwd: hex_md5($("#passwd-input").val()),
                role: roleMap[$("#role-tab li.active a").text()]
            };
            $.post("/Security/doLogin", data, function (rdata) {
                if (rdata.indexOf("success") != 0) {
                    $("#alert2").hide();
                    $("#alert1").show();
                }
                else 
                    location.href = rdata.substring(8);
                    
            });
        }
    });

    //输入框回车登录
    $("body").keydown(function () {
        if (event.keyCode == 13) {
            $("#login-btn").click();
        }
    })
})
