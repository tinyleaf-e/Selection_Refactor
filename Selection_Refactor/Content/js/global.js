$(document).ready(function () {

    //菜单的点击事件
    $(".page-tab").click(function () {
        var accessStage = $(this).attr("access-stage");
        var errorMsg = $(this).attr("error-msg");
        if (!errorMsg)
            errorMsg = "该页面未在开放时间内";
        var url = location.origin + $(this).attr("href");
        if (accessStage[stage - 1] == '1')
            location.href = url;
        else
            $.gyAlert({
                title: "提示",
                contentText: errorMsg
            });
    });
})

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