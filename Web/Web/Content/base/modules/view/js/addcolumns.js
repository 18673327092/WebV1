(function ($, window, document) {
    var obj = function () {
        this.init();
        this.bind();
        this.initgrid();
        this.grid;
    }
    obj.prototype = {
        init: function () {
            var _this = this;
        },
        bind: function () {
            var _this = this;

        },
        initgrid: function () {
            var _this = this;
            //加载Grid
            _this.grid = new pagegride({
                columns: [
                            {
                                field: "Title",
                                title: "显示名称",
                                width: 150,
                            },
                             {
                                 field: "Name",
                                 title: "名称",
                                 hidden: true,
                             }
                             ,
                             {
                                 field: "FieldType",
                                 title: "类型",
                             },
                             {
                                 field: "FieldSql",
                                 title: "FieldSql",
                                 hidden: true,
                             },
                             {
                                 field: "JoinTableName",
                                 title: "JoinTableName",
                                 hidden: true,
                             },
                             {
                                 field: "OnSql",
                                 title: "OnSql",
                                 hidden: true,
                             },
                             {
                                 field: "OpenLink",
                                 title: "OpenLink",
                                 hidden: true,
                             },

                             {
                                 field: "TemplateSql",
                                 title: "TemplateSql",
                                 hidden: true,
                             },
                             {
                                 field: "Template",
                                 title: "Template",
                                 hidden: true,
                             },
                              {
                                  field: "Columns_Format",
                                  title: "Columns_Format",
                              }
                              ,
                              {
                                  field: "Type",
                                  title: "Type",
                               
                              }
                ],
                url: "/View/_FieldListByEntity",
                ispage: false,
                searchcontrols: [{ field: "eid" }, { field: "ceid" }]
            });
        },
        search: function () {
            var _this = this;
            var grid = $('#grid').data('kendoGrid');
            grid.dataSource.page(1);
        }
    }
    $.addcolumns = obj;
})($, window, document)
var addcolumns = new $.addcolumns();