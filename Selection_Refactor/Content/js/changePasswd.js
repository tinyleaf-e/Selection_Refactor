/*
 * Create By 罗欣、高晔
 * 双选系统的修改密码功能
 */
function changePasswd() {
    var timestamp = new Date().getTime(); //获取当前时间戳，目的是为了产生一个不同的不冲突的id
    var modalTemplate =
        '<!--模态框-->'
        +'<div class="modal fade"  id="myModal-' + timestamp + '">'
            +'<div class="modal-dialog">'
                +'<div class="modal-content">'
                    +'<div class="modal-header">'
                        + '<h4 class="modal-title">修改密码</h4>'
                    +'</div>'
                    +'<div class="modal-body">'
                        +'<div id="oldpwd" class="form-group">'
                            +'<label class="control-label">'
                                +'<i id="old-passwd-icon"></i>'
                                +'旧密码</label>'
                            +'<input class="form-control" type="password" id="old-passwd-input" />'
                        +'</div>'
                        +'<div id="newpwd" class="form-group">'
                            +'<label class="control-label">'
                                +'<i id="new-passwd-icon"></i>'
                                +'新密码</label>'
                            +'<input class="form-control" type="password" id="new-passwd-input" />'
                        +'</div>'
                        +'<div id="checkpwd" class="form-group">'
                            +'<label class="control-label">'
                                +'<i id="check-passwd-icon"></i>'
                                +'确认密码</label>'
                            +'<input class="form-control" type="password" id="check-passwd-input" />'
                        +'</div>'
                    +'</div>'
                    +'<div class="modal-footer">'
                        +'<button type="button" cancel class="btn btn-default">关闭</button>'
                        +'<button type="button" confirm class="btn btn-primary">提交更改</button>'
                    +'</div>'
                + '</div>'
            + '</div >'
        +'</div>'

    $("body").append(modalTemplate); //在body元素末尾添加这个模态框（此时不显示）
    var theModal = $("#myModal-" + timestamp);//以变量theModal表示这个模态框

    theModal.find(".modal-footer").find("button[cancel]").click(function () {//找到模态框尾部的取消按钮，绑定它的点击事件
        theModal.modal('hide');//隐藏模态框
        setTimeout(function () {
            theModal.remove();
        }, 1000);//1秒后删除这个临时模态框
    });

    theModal.find(".modal-footer").find("button[confirm]").click(function () {//找到模态框尾部的确认按钮，绑定它的点击事件
        var oldPasswdInput = theModal.find(".modal-body").find("input#old-passwd-input");//以变量oldPasswdInput模态框中的旧密码输入框
        var newPasswdInput = theModal.find(".modal-body").find("input#new-passwd-input");
        var checkPasswdInput = theModal.find(".modal-body").find("input#check-passwd-input");
        if (oldPasswdInput.val() == "") {
            $("#oldpwd").attr("class", "form-group has-error");
            $('#old-passwd-icon').attr("class", "fa fa-times-circle-o");
        }
        else {
            $("#oldpwd").attr("class", "form-group has-success");
            $('#old-passwd-icon').attr("class", "fa fa-check");
            if (oldPasswdInput.val() == newPasswdInput.val() || newPasswdInput.val() == "") {
                $('#newpwd').attr("class", "form-group has-error");
                $('#new-passwd-icon').attr('class', 'fa fa-times-circle-o');
            }
            else {
                $('#newpwd').attr("class", "form-group has-success");
                $('#new-passwd-icon').attr('class', 'fa fa-check');
                if (newPasswdInput.val() == checkPasswdInput.val() && newPasswdInput.val() != "") {
                    $('#checkpwd').attr("class", "form-group has-success");
                    $('#check-passwd-icon').attr('class', 'fa fa-check');
                    //如果验证成功，向后端发送post请求
                    var data = {
                        oldPasswd: oldPasswdInput.val(),
                        newPasswd: newPasswdInput.val()
                    }
                    $.post("/", data, function (rdata) {
                        //rdata为后端返回的数据
                        if (rdata == "表示验证成功的字符串") {//若修改成功
                            $.gyAlert({ //模态框插件来提示错误信息
                                title: "修改成功",
                                contentText: "您的密码已修改成功，请重新登录",
                                cancelButton: false
                            });
                            theModal.modal('hide');
                            setTimeout(function () {
                                theModal.remove();
                            }, 1000);
                        }
                        else {//若修改失败
                            $.gyAlert({
                                title: "修改失败",
                                contenttext: "修改失败的原因，在rdata中",
                                cancelbutton: false
                            });
                            theModal.modal('hide');
                            setTimeout(function () {
                                theModal.remove();
                            }, 1000);
                        }
                    })
                }
                else {
                    $('#checkpwd').attr("class", "form-group has-error");
                    $('#check-passwd-icon').attr('class', 'fa fa-times-circle-o');
                }
            }
        }

    });
    theModal.modal('show');//模态框显示
}


// <button onclick="changePasswd()">修改密码</button>