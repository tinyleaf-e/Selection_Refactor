//用于模版页面渲染，为了提高速度，单独写了这个文件
try {
    $(".page-tab." + currentPageTab).addClass("active");
} catch (e) {
    console.log("网站发生错误，请联系网站管理员");
}