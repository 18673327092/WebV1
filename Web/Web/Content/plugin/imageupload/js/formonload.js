$(function () {
    
    if ($('.gridly')) {
        $('.gridly').gridly({
            base: 40, // px 
            gutter: 20, // px
            columns: 12
        });
    }
})

function deleteimg(id) {
    if (confirm("确定删除该图片吗？")) {
        $.post("/Images/DeleteImg", { id: id }, function (data) {
           
        }, "json");
        $("div[data-id=" + id + "]").remove();
    }
}