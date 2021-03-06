﻿$(document).ready(function () {

    //菜单的点击事件
    $(".page-tab").click(function () {
        var accessStage = $(this).attr("access-stage");
        var errorMsg = $(this).attr("error-msg");
        if (!errorMsg)
            errorMsg = "该页面未在开放时间内";
        var url = location.origin + $(this).find("a").first().attr("to");
        if (stage > 0) {
            if (accessStage[stage - 1] == '1')
                 location.href = url;
            else
                $.gyAlert({
                    title: "提示",
                    contentText: errorMsg
                });
        } 
        else
            $.gyAlert({
                title: "提示",
                contentText: "系统已关闭，无法访问"
            });
    });

    $('input').bind('input propertychange', function () {
        $(this).removeClass("error-input");
    });

    logPage();
})

function logPage() {
    var logServerUrl = "http://10.251.254.211:8011/log";
    var projectTicket = "ae9cccfd-4d9d-4f46-8598-af4856fdfd78";
    var formatId = "48fbc64f-da79-473b-bc4c-5a5c4fb7a30d";
    var role = "";
    var userid = "";
    var page = location.pathname;
        var roleReg = new RegExp("(?:^|&)role=(.*?)(?:&|$)");
        var userIdReg = new RegExp("(?:^|&|=)userId=(.*?)(?:&|$)");
        if (arr = document.cookie.match(roleReg)) {
            role = unescape(arr[1])
        }
        if (arr = document.cookie.match(userIdReg)) {
            userid = unescape(arr[1])
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
