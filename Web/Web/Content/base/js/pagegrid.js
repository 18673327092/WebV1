
function pagegride(_options) {
    var _this = this;
    var _primary = "ID";
    if (_options.primary != "") {
        _primary = _options.primary;
    };
    var options = function () {
        return {
            id: "grid",
            url: "_List",
            searchcontrols: [{ field: "Name" }],
            columns: [
                        {
                            field: _primary,
                            headerTemplate: "<input ID='mastercheckbox' type='checkbox'/>",
                            headerAttributes: { style: "text-align:center" },
                            attributes: { style: "text-align:center" },
                            template: "<input type='checkbox' value='#=" + _primary + "#' class='checkboxGroups'/>",
                            width: 40
                        },
                         {
                             field: "Name",
                             title: "名称",
                         }
            ],
            selectedids: [8, 9, 10],
            pageSize: 15,
            ischeckbox: true,
            ispage: true,
            paramsdata: {},
            isselectable: true,
            click: function () { },
            dbclick: function () { },
        };
    }();
    $.selectedids = options.selectedids;
    var selectedids = options.selectedids;
    $.extend(options, _options || {});
    if (options.ischeckbox) {
        options.columns.unshift(
          {
              field: "",
              headerTemplate: "<input ID='mastercheckbox' type='checkbox'/>",
              headerAttributes: { style: "text-align:center" },
              attributes: { style: "text-align:center" },
              template: "<input type='checkbox' name='# var ids = [6, 4, 5]; if($.selectedids.indexOf(" + _primary + ")>-1) {#chk#}#'  value='#=" + _primary + "#' class='checkboxGroups'/>",
              width: 40,
              filterable: false,
              sortable: false,
              columnMenu: false,
              resizable: false,
          });
    };

    var model_fields = {};
    $.each(_options.columns, function (m, n) {
        if (n.type != null) {
            model_fields[n.field] = new Object();
            model_fields[n.field]["type"] = n.type;
        }
    });

    var pageable;
    if (options.ispage) {
        pageable = function () {
            return {
                refresh: true,
                pageSizes: [5, 10, 15, 20, 30, 50, 100, 500],
                buttonCount: 10,
                messages: {
                    display: "显示{0}-{1}条，共{2}条",
                    empty: "没有数据",
                    page: "页",
                    of: "/ {0}",
                    itemsPerPage: "条/页",
                    first: "第一页",
                    previous: "前一页",
                    next: "下一页",
                    last: "最后一页",
                    refresh: "刷新"
                }
            }
        }();
    } else {
        options.pageSize = 99999;
    };

    var dataSource = function () {
        return {
            type: "json",
            transport: {
                read: {
                    url: options.url,
                    type: "POST",
                    dataType: "json",
                    data: function () {
                        var data = {};
                        if (options.paramsdata) {
                            $.extend(data, options.paramsdata || {});
                        }
                        if (options.searchcontrols != null) {
                            $.each(options.searchcontrols, function (m, n) {
                                data[n.field] = $("#" + n.field).val();
                            })
                        }
                        // addAntiForgeryToken(data);
                        return data;
                    }
                }
            },
            schema: {
                data: "Data",
                total: "Total",
                errors: "Errors",
                model: { fields: model_fields }
            },
            error: function (e) {
                //  display_kendoui_grid_error(e);
                //console.log(JSON.stringify(e));
                // Cancel the changes
                this.cancelChanges();
            },
            pageSize: options.pageSize,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        }
    }();
    var columns = [];
    $.extend(columns, _options.columns || {});
    columns.push(
      {
          field: "",
          title: "",

      });
    var grid_options = {
        dataSource: dataSource,
        pageable: pageable,
        columns: columns,
    };
    if (options.isselectable) {
        grid_options.selectable = "row"
    }

    if ("boolean" == (typeof options.filterable) || "object" == (typeof options.filterable)) {
        grid_options.filterable = options.filterable
    };
    if ("boolean" == (typeof options.sortable) || "object" == (typeof options.sortable)) {
        grid_options.sortable = options.sortable
    };
    if ("boolean" == (typeof options.columnMenu) || "object" == (typeof options.columnMenu)) {
        grid_options.columnMenu = options.columnMenu
    };
    if ("boolean" == (typeof options.resizable) || "object" == (typeof options.resizable)) {
        grid_options.resizable = options.resizable
    };
    kendo.culture("zh-CN");
    _this.grid = $("#" + options.id).kendoGrid(grid_options);
    _this.refresh = function () {
        $(".k-pager-refresh").click()
    };
    _this.getcheckedid = function () {
        var _selectids = [];
        $(".checkboxGroups:checked").each(function (m, n) { _selectids.push(parseInt($(n).val())); });
        return _selectids;
    };

    _this.getcheckedrows = function () {
        var _rows = [];
        var row = _this.grid.data("kendoGrid").select();
        $.each(row, function (m, n) {
            _rows.push(JSON.parse(JSON.stringify(_this.grid.data("kendoGrid").dataItem(this))));
        });
        return _rows;
    };

    _bind = function () {

        //全选
        $("#mastercheckbox").click(function () {
            var _checked = $(this).is(":checked");
            if (!_checked) {
                $(".checkboxGroups").each(function (i, e) {
                    $(e).parent("td").parent("tr").removeAttr("aria-selected").removeClass("k-state-selected");
                });
                $(".checkboxGroups").iCheck("uncheck");
            } else {
                $(this).attr({ "aria-selected": true }).addClass("k-state-selected");
                $(".checkboxGroups").each(function (i, e) {
                    $(e).parent("td").parent("tr").attr({ "aria-selected": true }).addClass("k-state-selected").removeClass("movecurr");
                });
                $(".checkboxGroups").iCheck("check");
            }
        });

        _this.grid.on('click', '.k-grid-content tr', function (event) {
            var chk = $(this).find("input[type=checkbox]");
            if (event.target.className == "checkboxGroups" || $(event.target).find(".checkboxGroups").length > 0) {
                if ($(event.target).find(".checkboxGroups").length > 0) {
                    chk.click(); return false;
                }
                if (chk.is(":checked")) {
                    $(this).attr({ "aria-selected": true }).addClass("k-state-selected");
                } else {
                    chk.iCheck("checked");
                    $(this).removeAttr("aria-selected").removeClass("k-state-selected");
                }

            } else {
                if ($(event.target).find(".checkboxGroups").length > 0) {

                } else {
                    $(".checkboxGroups").iCheck("uncheck");
                }
                var row = _this.grid.data("kendoGrid").select();
                $(row).find("input[type=checkbox]").iCheck("check");
            };
            $.each($(".checkboxGroups:checked"), function (m, n) {
                $(n).parents("tr").attr({ "aria-selected": true }).addClass("k-state-selected");
            });
            $(this).removeClass("movecurr");
            if (typeof options.click == "function") {
                var row = _this.grid.data("kendoGrid").select();
                var data = _this.grid.data("kendoGrid").dataItem(row);
                options.click(data);
            };
        });

        _this.grid.on('dblclick', '.k-grid-content tr', function () {
            //// 获取当前选择行数据                     
            var row = _this.grid.data("kendoGrid").select();
            var data = _this.grid.data("kendoGrid").dataItem(row);
            if (typeof options.dbclick == "function") {
                options.dbclick(data);
            }
        });

        _this.grid.on('mouseover', '.k-grid-content tr', function () {
            //获取当前选择行数据
            if (!$(this).hasClass("k-state-selected")) {
                $(this).addClass("movecurr").siblings().removeClass("movecurr");
            } else {
                $(this).siblings().removeClass("movecurr");
            }

        });
    }();

    return _this;
}


