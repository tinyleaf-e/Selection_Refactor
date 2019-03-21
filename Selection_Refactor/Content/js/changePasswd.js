/*
 * Create By 罗欣、高晔
 * 双选系统的修改密码功能
 */
function changePasswd() {
    var timestamp = new Date().getTime(); //获取当前时间戳，目的是为了产生一个不同的不冲突的id
    var modalTemplate = "";//模态框对应的HTML代码

    $("body").append(modalTemplate); //在body元素末尾添加这个模态框（此时不显示）
    var theModal = $("#myModal-" + timestamp);//以变量theModal表示这个模态框

    theDialog.find(".modal-footer").find("button[cancel]").click(function () {//找到模态框尾部的取消按钮，绑定它的点击事件
        theDialog.modal('hide');//隐藏模态框
        setTimeout(function () {
            theDialog.remove();
        }, 1000);//1秒后删除这个临时模态框
    });

    theDialog.find(".modal-footer").find("button[confirm]").click(function () {//找到模态框尾部的确认按钮，绑定它的点击事件
        var oldPasswdInput = theDialog.find(".modal-body").find("button#old-passwd-input");//以变量oldPasswdInput模态框中的旧密码输入框
        //依上述方法找到其他需要的元素

        //进行输入框内容的合法性验证，验证失败后提示并return掉

        //如果验证成功，向后端发送post请求
        var data = { //要发送的数据，json格式
            oldPasswd: "",
            newPasswd: ""
        };
        $.post("url", data, function (rdata) {
            //rdata为后端返回的数据
            if (rdata == "表示验证成功的字符串") {//若修改成功
                $.gyAlert({ //模态框插件来提示错误信息
                    title: "修改成功",
                    contentText: "您的密码已修改成功，请重新登录",
                    cancelButton: false
                });
                theDialog.modal('hide');
                setTimeout(function () {
                    theDialog.remove();
                }, 1000);
            }
            else {//若修改失败
                $.gyAlert({
                    title: "修改失败",
                    contentText: "修改失败的原因，在rdata中",
                    cancelButton: false
                });
            }
        })
    });
    theDialog.modal('show');//模态框显示
}