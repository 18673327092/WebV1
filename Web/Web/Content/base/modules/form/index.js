
var formsetting = {
    entityid: 0,
    OneRowFieldsNumber: 3,
    init: function () {
        this.bind();
    },
    bind: function () {
        var _this = this;
        $("#main-form").find("tr").find("td").on("click", function () {
            $("#main-form").find("tr").removeClass("tdhover");
            $("#main-form").find("td").removeClass("tdhover");
            $(this).addClass("tdhover");
        });

        $("#btn-save").on("click", function () {
            var fields = [];
            var sort = 1;
            $("#main-form tr").each(function (y, tr) {
                var $tr = $(this);
                var colspan = $tr.find("td").length;
                $tr.find("td").each(function (x, td) {
                    if ($(td).html() != "") {
                        var control = $(td).find(".filed");
                        fields.push(
                            {
                                id: $(control).data("id"),
                                x: x + 1,
                                y: y + 1,
                                sort: sort,
                                colspan: $(td).attr("colspan") ? $(td).attr("colspan") : 1
                            });
                    }
                    sort++;
                })
            });
            console.log(JSON.stringify(fields))
            $.post("/Form/_Save", { strfields: JSON.stringify(fields), entityid: _this.entityid },
                function (data) {
                    if (data.Success) {
                        dlg.msg.success("保存成功");
                    } else if (data.Message) {
                        dlg.msg.success(data.Message);
                    }
                })
        });

        function DeleteField() {

            var deletd = $(".tdhover");
            if (deletd.html().trim() == "") {
                deletd.siblings("td").attr("colspan", _this.OneRowFieldsNumber);
                deletd.parent().find("td").addClass("tdhover")
                if (deletd.parent().find("td").siblings().length == 0) {
                    deletd.parent().find("td").parent("tr").remove();
                }
                deletd.remove();
            } else {
                $("#allfied-tbody").append($("<tr><td>" + deletd.html() + "</td></tr>"))
                var rd = REDIPS.drag;
                rd.init("drag");
                deletd.find("div").remove();
            }
        }

        //删除选中
        $("#delete-selected").on("click", function () {
            DeleteField();
            return;
            var deletd = $(".tdhover");
            if (deletd.html().trim() == "") {
                deletd.siblings("td").attr("colspan", _this.OneRowFieldsNumber);
                deletd.parent().find("td").addClass("tdhover")
                if (deletd.parent().find("td").siblings().length == 0) {
                    deletd.parent().find("td").parent("tr").remove();
                }
                deletd.remove();
            } else {
                $("#allfied-tbody").append($("<tr><td>" + deletd.html() + "</td></tr>"))
                var rd = REDIPS.drag;
                rd.init("drag");
                deletd.find("div").remove();
            }
        });

        //选中标签
        $(".drag").on("click", function () {
            $("#main-form").find("tr").removeClass("tdhover");
            $("#main-form").find("td").removeClass("tdhover");
            $(this).parent("td").addClass("tdhover");
        });

        //新增行
        $("#btn-add1").on("click", function () {
            _this.append(1);
        });

        //新增行
        $("#btn-add2").on("click", function () {
            _this.append(formsetting.OneRowFieldsNumber);
        });

        //Dele键盘事件
        $(document).keydown(function (event) {
            if (event.keyCode == 46) {
                DeleteField();
            }
        });
    },
    append: function (num) {
        var $tr = $("<tr></tr>");
        for (var i = 1; i <= num; i++) {
            $tr.append("<td></td>");
        }
        //switch (num) {
        //    case 1:
        //        tr = $("<tr><td  colspan='2'></td></tr>");
        //        break;
        //    case 2:
        //        tr = $("<tr><td></td><td></td><td></td></tr>");
        //        break;
        //}
        var $selecttr = $(".tdhover").parent("tr");
        if ($selecttr.length > 0) {
            $tr.insertAfter($selecttr)
        } else {
            $("#main-form").append($tr)
        }
      
        $("#main-form").find("tr").find("td").on("click", function () {
            $("#main-form").find("td").removeClass("tdhover");
            $(this).addClass("tdhover");
        });
    }
}
$(function () {
    formsetting.init();
})
