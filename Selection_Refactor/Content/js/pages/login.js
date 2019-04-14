$(document).ready(function () {
    $("#login-btn").click(function () {
        var roleMap = {
            "学生": 1,
            "教师": 2,
            "教务": 3,
            "管理员": 4
        }
        if ($("#userid-input").val() == "" || $("#passwd-input").val() == "")
            $.gyAlert({
                title: "提示",
                contentText: "用户名或密码不能为空"
            });
        else {
            var data = {
                userId: $("#userid-input").val(),
                passwd: $("#passwd-input").val(),
                role: roleMap[$("#role-tab li.active a").text()]
            };
            $.post("/Security/doLogin", data, function (rdata) {
                if (rdata.indexOf("success")!=0)
                    $.gyAlert({
                        title: "登录失败",
                        contentText: rdata
                    });
                else
                    location.href = rdata.substring(8)
            });
        }
    });

    //输入框回车登录
    $("#passwd-input").keydown(function () {
        if (event.keyCode == 13) {
            $("#login-btn").click();
        }
    })
})
