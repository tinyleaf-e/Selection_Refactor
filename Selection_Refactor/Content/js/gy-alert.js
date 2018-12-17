/**
 * Created by 高晔 on 2017/7/29.
 */
(function ($) {
    $.extend({
        "gyAlert":function (options) {
            var opts=$.extend({},defaults,options);
            //this.each(function () {
                //var $this=$(this);
                var timestamp=new Date().getTime();
                $("body").append(
                '<!--GY Modal Dialog -->'
                +'<div class="modal fade" id="myModal-'+timestamp+'">'
                +'<div class="modal-dialog" style="top: 100px">'
                +' <div class="modal-content">'
                +'<div class="modal-header" style="padding: 8px 15px">'
                +'<h4 class="modal-title bk-fg-primary">'+opts.title+'</h4>'
                +'</div>'
                +'<div class="modal-body">'
                +(opts.contentHtml==""?'<p style="'+opts.contentStyle+'">'+opts.contentText+'</p>':opts.contentHtml)
                +'</div>'
                +'<div class="modal-footer" style="padding: 8px 15px">'
                +(opts.cancelButton==true?'<button type="button" cancel style="margin-right: 10px" class="btn btn-'+opts.cancelButtonStyle+' btn-sm" data-dismiss="modal">'+opts.cancelButtonText+'</button>':'')
                +'<button type="button" confirm class="btn btn-'+opts.confirmButtonStyle+' btn-sm">'+opts.confirmButtonText+'</button>'
                +'</div>'
                +'</div>'
                +'</div>'
                +'</div>'
                +'<!-- End GY Modal Dialog -->'
                );
                var theDialog=$("#myModal-"+timestamp);
                theDialog.find(".modal-footer").find("button[cancel]").click(function () {
                    if(opts.missAfterClick==true)
                    {
                        theDialog.modal('hide');
                    }
                    opts.cancel(theDialog);
                    if(opts.missAfterClick==true)
                    {
                        theDialog.modal('hide');
                        setTimeout(function () {
                            theDialog.remove();
                        },1000);
                    }
                });
                theDialog.find(".modal-footer").find("button[confirm]").click(function () {
                    if(opts.missAfterClick==true)
                    {
                        theDialog.modal('hide');
                    }
                    opts.confirm(theDialog);
                    if(opts.missAfterClick==true)
                    {
                        theDialog.modal('hide');
                        setTimeout(function () {
                            theDialog.remove();
                        },1000);
                    }
                });
                theDialog.modal('show');
            //});

        }
    });
    var defaults={
        title:'确认',
        contentText:'你确认进行该操作吗？',
        contentHtml:'',
        contentStyle:'',
        cancelButton:true,
        cancelButtonText:'返 回',
        cancelButtonStyle:'default',
        cancel:function () {

        },
        confirmButtonText:'确 认',
        confirmButtonStyle:'primary',
        confirm:function () {

        },
        missAfterClick:true
    };
})(window.jQuery);
//没特殊要求的话，按下边这个模板就够了
// $.gyAlert({
//    title:"左上角的标题",
//     contentText:"body中的内容",
//     confirm:function () {
//         //点确定之后的操作
//     }
// });