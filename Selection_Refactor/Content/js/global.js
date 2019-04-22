$(document).ready(function () {

    //菜单的点击事件
    $(".page-tab").click(function () {
        var accessStage = $(this).attr("access-stage");
        var errorMsg = $(this).attr("error-msg");
        if (!errorMsg)
            errorMsg = "该页面未在开放时间内";
        var url = location.origin + $(this).find("a").first().attr("to");
        if (accessStage[stage - 1] == '1')
            location.href = url;
        else
            $.gyAlert({
                title: "提示",
                contentText: errorMsg
            });
    });

    $('input').bind('input propertychange', function () {
        $(this).removeClass("error-input");
    });

    logPage();
})

function logPage() {
    var logServerUrl = "http://10.251.254.198:8011/log";
    var projectTicket = "ae9cccfd-4d9d-4f46-8598-af4856fdfd78";
    var formatId = "48fbc64f-da79-473b-bc4c-5a5c4fb7a30d";
    var role = "";
    var userid = "";
    var page = location.pathname;
    var arr, reg = new RegExp("(^| )" + "account" + "=([^;]*)(;|$)");

    if (arr = document.cookie.match(reg)) {
        var content = unescape(arr[2]);
        var roleReg = new RegExp("(^| )" + "role" + "=([^;]*)(;|$)");
        var userIdReg = new RegExp("(^| )" + "userId" + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(roleReg)) {
            role = unescape(arr[2])
        }
        if (arr = document.cookie.match(userIdReg)) {
            userid = unescape(arr[2])
        }
    }
    var logData = {
        role: role,
        userid: userid,
        page: page
    };
    _LogWrite(logServerUrl, projectTicket, formatId, logData);
}

function logout() {
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    var keys = document.cookie.match(/[^ =;]+(?=\=)/g);
    if (keys) {
        for (var i = keys.length; i--;)
            document.cookie = keys[i] + "=0; expire=" + date.toGMTString() + "; path=/";
    }
    location.href = "/";
}
