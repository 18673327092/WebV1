var relationfieldselectpanelmulti = {
    values: [],
    names: [],
    objs: [],
    init: function (options) {
        var _this = this;
        _this.options = {
            id: "",
            uid: 0,
            index: parent.layer.getFrameIndex(window.name),
            ischeckbox: true
        };
        $.extend(_this.options, options || {})
        _this._rendergrid();
        _this.bind();
        _this.initvalue();
        _this.autogridsize();
    },
    initvalue: function () {
        var _this = this;
        $("#select_value>span").each(function (m, n) {
            var name = $(n).data("name");
            var id = $(n).data("id");
            _this.names.push(name);
            _this.values.push(id);
        });
        $(".select_span").on("click", function (e) {
            $(this).addClass("select_value_curr").siblings().removeClass("select_value_curr");
            $("#btn-remove").show();
        })
    },
    select: function () {
        var _this = this;
        if (_this.options.ischeckbox == false) {
            _this.names = [];
            _this.values = [];
            _this.objs = [];
            $("#select_value").html("");
        }
        debugger
        $.each(_this.grid.getcheckedrows(), function (z, data) {
            var _id = data[_this.options.relationentity + "$ID"];
            var _name = data[_this.options.relationentity + "$Name"];
            var tag = '<span class="select_span" data-name="' + _name + '" data-id="' + _id + '" >\
                                <span class="title"><i class="fa icon-ok-circle"></i>' + _name + '</span>\
                                <input type="hidden" class="value" value="' + _id + '" />\
                              </span>'
            var ishave = false;
            $.each(_this.values, function (m, n) {
                if (_id == n) ishave = true;
            })
            if (!ishave) {
                _this.values.push(_id);
                _this.names.push(_name);
                $("#select_value").append(tag);
                var obj = {};
                for (var d in data) {
                    obj[d] = data[d]
                }
                _this.objs.push(obj);
            }
        });
        $(".select_span").on("click", function (e) {
            $(this).addClass("select_value_curr").siblings().removeClass("select_value_curr");
            $("#btn-remove").show();
        });

        $(".select_span").on("dblclick", function (e) {
            _this.remove();
        });
    },
    remove: function () {
        var _this = this;
        var select_value_curr = $(".select_value_curr");
        $.each(select_value_curr, function (m, n) {
            _this.values.remove($(n).find("input").val())
            _this.names.remove($(n).find("span").text())
        })
        select_value_curr.remove();
        $("#btn-remove").hide();
    },
    remove_all: function () {
        var _this = this;
        $("#select_value").html("");
        _this.values = [];
        _this.names = [];
        $("#btn-remove").hide();
    },
    ok: function () {
        var _this = this;
        //判断来源页面是否是列表
        if ($("#page").val() == "list") {
            parent.$('#span_' + _this.options.id).text(parent.$('#span_' + _this.options.id).data("title") + ": " + _this.names.join(', '));
        } else {
            parent.$('#span_' + _this.options.id).text(_this.names.join(', '));
        }
        parent.$('#input_' + _this.options.id).val(_this.values.join(','));
        parent.$('#icon_' + _this.options.id).data("value", _this.values.join(','));
        parent.$('#span_' + _this.options.id).data("value", _this.values.join(','));
        if (_this.names.length == 0) {
            parent.$('#span_' + _this.options.id).text(parent.$('#span_' + _this.options.id).data("title"));
        }
        var callobj = {
            FieldId: $("#FieldId").val(),
            Tiele: _this.names.join(','),
            value: _this.values.join(','),
            obj: JSON.stringify(_this.objs)
        };
        if (typeof parent.DialogCloseCallbak == "function") {
            parent.DialogCloseCallbak(callobj);
        }
        _this.cancel();
    },
    cancel: function () {
        var _this = this;
        parent.layer.close(_this.options.index);
    },
    search: function () {
        var _this = this;
        $("#issearch").val(true);
        var _filter = [];
        $.each($(".SearchField"), function () {
            var _o = {
                field: $(this).data("field"),
                type: $(this).data("type"),
                opera: $(this).data("opera"),
                sort: $(this).data("sort"),
                sql: $(this).data("sql"),
            };
            var value = $(this).val();
            if (typeof value == "string") {
                value = value.trim()
            }
            if (value instanceof Array) {
                value = value.join(",");
            }
            if (_o.type == "sql") {
                value = _o.sql;
            }
            if (value != "" && value != null) {
                _o.value = value;
                _filter.push(_o);
            }
        });
        $("#Filter").val(JSON.stringify(_filter));
        _this.grid.refresh();
        $("#issearch").val(false)
    },
    bind: function () {
        var _this = this;
        $("#btn-save").on("click", function () { _this.ok(); })
        $("#btn-cancel").on("click", function () { _this.cancel(); })
        $("#btn-remove").on("click", function () { _this.remove(); })
        $("#btn-remove-all").on("click", function () { _this.remove_all(); })
        $("#btn-search").on("click", function () { _this.search(); })
        $("#btn-select").on("click", function () { _this.select(); })
        $(".SearchField").keydown(function (event) {
            event = document.all ? window.event : event;
            if ((event.keyCode || event.which) == 13) { _this.search() }
        });
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
        $("#btn-reset").click(function () {
            $.each($("#search-panel").find(".SearchField"), function () {
                $(this).val("");
            });
            $("#search-panel").find(".search-span").each(function () {
                $(this).text($(this).data("title"));
            });
            $("#search-panel").find(".search-icon").each(function () {
                $(this).data("value", "");
            });
            $("#search-panel").find("select").selectpicker('deselectAll');
            _this.search();
        });

        $(".select_span").on("dblclick", function (e) {
            _this.remove();
        });
    },
    autogridsize: function () {
        var _this = this;
        $("#dicgrid").height($("#height").val() - parseInt($("#search-panel").height() || 0));
        _this.search();
    },
    _rendergrid: function () {
        var _this = this;
        //列表数据绑定
        _this.grid = new pagegride({
            id: "dicgrid",
            url: 'RelationAjaxList',
            columns: JSON.parse($("#columns").val()),
            paramsdata: { vid: $("#vid").val(), eid: $("#eid").val(), WhereSql: $("#WhereSql").val().trim() },
            params: { selectable: "multiple" },
            searchcontrols: [{ field: "KeyWord" }, { field: "issearch" }, { field: "Filter" }],
            pageSize: 10,
            resizable: true,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            ischeckbox: _this.options.ischeckbox,
            dbclick: function () {
                _this.select();
                if (_this.options.ischeckbox == false) {
                    _this.ok();
                }
            },
            click: function () {
                if (_this.options.ischeckbox == false) {
                    _this.select();
                }
            }
        });
    },
    open_field_dialog: function (e) {
        var value = $(e).data("value");
        var fieldid = $(e).data("fieldid");
        var id = $(e).data("id");
        dlg.openframe({
            title: "选择",
            offset: ['10px'],
            type: 2,
            //btn: ['确定', '取消'],
            //shade: 0,
            area: ['80%', '368px'],
            fixed: true, //不固定
            maxmin: false,
            content: "/View/RelationFieldSelectPanelMulti?height=190&fieldid=" + fieldid + "&value=" + ((typeof value) != "undefined" ? value : "") + "&id=" + id,
            yes: function (index) {
                parent.layer.close(index);
            },
        });
    },
}

function totips(value) {
    if (value.toString().length > 10) {
        return '<span class="tooltip" title="' + value + '">' + (value.toString().substring(0, 10) + "...") + '</span>';
    } else { return value; }
}
