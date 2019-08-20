var report = {
    init: function () {
        var _this = this;
        _this.renderlist();
        _this.autoframe();
        _this.load();
        _this.bind();
    },
    load: function () {
        var selectallinterval = setInterval(function () {
            if ($(".bs-select-all").length > 0) {
                clearInterval(selectallinterval);
            }
            $(".bs-select-all").text("全选").css({ "width": "100%" });
            $(".bs-deselect-all").hide();
            $(".bs-select-all").on("click", function () {
                if ($(this).text() == "全选") {
                    $(this).text("取消全选");
                } else {
                    $(this).next("button").click();
                    $(this).text("全选");
                    return false;
                }
            });
        }, 500);

    },
    bind: function () {
        var _this = this;
        $(".SearchField").keydown(function (event) {
            event = document.all ? window.event : event;
            if ((event.keyCode || event.which) == 13) {
                _this.search();
            }
        });
        $("#btn-search").on("click", function () {
            _this.search();
        })
        $(window).resize(function () {
            _this.autoframe();
        });

        $("#btn-export").click(function () {
            var loadindex = parent.layer.load(1);
            var index = parent.dlg.msg.info("导出中...");
            $('#ifile').attr("src", "/Report/Export?v=" + $("#ID").val());
            setTimeout(function () { parent.layer.close(loadindex); parent.layer.close(index) }, 3000);
        });
    },
    setfilter: function () {
        var _filter = [];
        $.each($("input[class*='SearchField'],select[class*='SearchField']"), function () {
            var _o = {
                field: $(this).data("field"),
                type: $(this).data("type"),
                opera: $(this).data("opera"),
                sort: $(this).data("sort"),
            };
            var value = $(this).val();
            if (value == "" || typeof value == 'undefined') value = $(this).data("defaultvalue");
            if (typeof value == "string") {
                value = value.trim()
            }
            if (value instanceof Array) {
                value = value.join(",");
            }
            _o.value = value;
            _filter.push(_o);
        })
        $("#Filter").val(JSON.stringify(_filter));
    },
    autoframe: function () {
        var _this = this;
        var _h = parseInt($("#search-panel").height());
        $("#grid").height($(window).height() - (46 + _h));
        _this.search();
    },
    search: function () {
        var _this = this;
        $("#issearch").val(true);
        _this.setfilter();
        _this.pg.refresh();
        $("#issearch").val(false)
    },
    renderlist: function () {
        //加载Grid
        this.pg = new pagegride({
            url: "/Report/_ReportList",
            paramsdata: { v: $("#ID").val() },
            searchcontrols: [{ field: "Filter" }, { field: "issearch" }],
            sortable: false,
            resizable: true,
            ischeckbox: false,
            isselectable: false,
            columns: JSON.parse($("#gridfields").val()),
        });
    }
}
$(function () {
    report.init();
});
function open_field_dialog(e) {
    var value = $(e).data("value");
    var fieldid = $(e).data("fieldid");
    var id = $(e).data("id");
    dlg.openframe({
        title: "选择",
        offset: ['10px'],
        type: 2,
        //btn: ['确定', '取消'],
        //shade: 0,
        area: ['80%', '520PX'],
        fixed: true, //不固定
        maxmin: false,
        content: "/View/RelationFieldSelectPanel?t=0&fieldid=" + fieldid + "&value=" + ((typeof value) != "undefined" ? value : "") + "&id=" + id,
        yes: function (index) {
            parent.layer.close(index);
        },
    });
}