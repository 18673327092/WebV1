$(document).ready(function () {
    Seach();
});

function Seach() {
    var dataSource = new kendo.data.TreeListDataSource({
        transport: {
            read: {
                url: "/AccessRightSetting/_PageConfigList?roleid=" + $("#hid_RoleID").val(),
                dataType: "json"
            }
        },
        schema: {
            model: {
                id: "MenuID",
                parentId: "ReportsTo",
                fields: {
                    MenuID: { type: "number", nullable: false },
                    ReportsTo: { nullable: true, type: "number" }
                },
                expanded: true,
                refresh: true,
            }
        }
    });

    var grid = $("#grid").kendoTreeList({
        dataSource: dataSource,
        id: "MenuID",
        columns: [
            { field: "MenuName", expandable: true, title: "菜单", width: 250 },
            //{
            //    width: 80,
            //    headerTemplate: "<label class='form-tag checkbox-inline'><input onClick='chkAll(this)' style='margin-left: -15px; margin-top: 1px;' data-type='chk_read' type='checkbox'/>查看</label>",
            //    headerAttributes: { style: "text-align:center" },
            //    attributes: { style: "text-align:center" },
            //    template: function (data) {
            //        if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("0,") != -1) {
            //            return " <input type='checkbox' class='chk_read'  name='chk'  value='0'" + (data.OperationCheck.indexOf("0,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //        }
            //    }
            //},
            //{
            //    width: 80,
            //    headerTemplate: "<label class='form-tag checkbox-inline'  ><input onClick='chkAll(this)' style='margin-left: -15px; margin-top: 1px;' data-type='chk_add' type='checkbox'/>新增</label>",
            //    headerAttributes: { style: "text-align:center" },
            //    attributes: { style: "text-align:center" },
            //    template: function (data) {
            //        if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("1,") != -1) {
            //            return " <input type='checkbox' class='chk_add' name='chk' value='1' " + (data.OperationCheck.indexOf("1,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //        }
            //    }
            //},
            //{
            //    width: 80,
            //    headerTemplate: "<label class='form-tag checkbox-inline' ><input  onClick='chkAll(this)'  style='margin-left: -15px; margin-top: 1px;' data-type='chk_edit' type='checkbox'/>修改</label>",
            //    headerAttributes: { style: "text-align:center" },
            //    attributes: { style: "text-align:center" },
            //    template: function (data) {
            //        if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("2,") != -1) {
            //            return " <input type='checkbox' class='chk_edit' name='chk' value='2' " + (data.OperationCheck.indexOf("2,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //        }
            //    }
            //},
            //{
            //    width: 90,
            //    headerTemplate: "<label class='form-tag checkbox-inline' ><input  onClick='chkAll(this)'  style='margin-left: -15px; margin-top: 1px;' data-type='chk_dele' type='checkbox'/>删除</label>",
            //    headerAttributes: { style: "text-align:center" },
            //    attributes: { style: "text-align:center" },
            //    template: function (data) {
            //        if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("3,") != -1) {
            //            return " <input type='checkbox'  class='chk_dele' name='chk'  value='3'" + (data.OperationCheck.indexOf("3,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //        }
            //    }
            //},
            // {
            //     width: 90,
            //     headerTemplate: "<label class='form-tag checkbox-inline' ><input  onClick='chkAll(this)'  style='margin-left: -15px; margin-top: 1px;' data-type='chk_disanle' type='checkbox'/>删除/还原</label>",
            //     headerAttributes: { style: "text-align:center" },
            //     attributes: { style: "text-align:center" },
            //     template: function (data) {
            //         if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("4,") != -1) {
            //             return " <input type='checkbox'  class='chk_disanle' name='chk'  value='4'" + (data.OperationCheck.indexOf("4,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //         }
            //     }
            // },
            //  {
            //      width: 60,
            //      headerTemplate: "<label class='form-tag checkbox-inline' ><input  onClick='chkAll(this)'  style='margin-left: -15px; margin-top: 1px;' data-type='chk_share' type='checkbox'/>共享</label>",
            //      headerAttributes: { style: "text-align:center" },
            //      attributes: { style: "text-align:center" },
            //      template: function (data) {
            //          if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("5,") != -1) {
            //              return " <input type='checkbox'  class='chk_share' name='chk'  value='5'" + (data.OperationCheck.indexOf("5,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //          }
            //      }
            //  },
            //   {
            //       width: 60,
            //       headerTemplate: "<label class='form-tag checkbox-inline' ><input  onClick='chkAll(this)'  style='margin-left: -15px; margin-top: 1px;' data-type='chk_assignment' type='checkbox'/>分派</label>",
            //       headerAttributes: { style: "text-align:center" },
            //       attributes: { style: "text-align:center" },
            //       template: function (data) {
            //           if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("6,") != -1) {
            //               return " <input type='checkbox'  class='chk_assignment' name='chk'  value='6'" + (data.OperationCheck.indexOf("6,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //           }
            //       }
            //   },
            //   {
            //       width: 60,
            //       hidden:true,
            //       headerTemplate: "<label class='form-tag checkbox-inline' ><input  onClick='chkAll(this)'  style='margin-left: -15px; margin-top: 1px;' data-type='chk_import' type='checkbox'/>导入</label>",
            //       headerAttributes: { style: "text-align:center" },
            //       attributes: { style: "text-align:center" },
            //       template: function (data) {
            //           if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("7,") != -1) {
            //               return " <input type='checkbox'  class='chk_import' name='chk'  value='7'" + (data.OperationCheck.indexOf("7,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //           }
            //       }
            //   },
            //   {
            //       width: 60,
            //       headerTemplate: "<label class='form-tag checkbox-inline' ><input  onClick='chkAll(this)'  style='margin-left: -15px; margin-top: 1px;' data-type='chk_export' type='checkbox'/>导出</label>",
            //       headerAttributes: { style: "text-align:center" },
            //       attributes: { style: "text-align:center" },
            //       template: function (data) {
            //           if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("8,") != -1) {
            //               return " <input type='checkbox'  class='chk_export' name='chk'  value='8'" + (data.OperationCheck.indexOf("8,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //           }
            //       }
            //   },
            //{
            //    width: 80,
            //    headerTemplate: "<label class='form-tag checkbox-inline'><input onClick='chkAll(this)' style='margin-left: -15px; margin-top: 1px;' data-type='chk_select' type='checkbox'/>高级查找</label>",
            //    headerAttributes: { style: "text-align:center" },
            //    attributes: { style: "text-align:center" },
            //    template: function (data) {
            //        if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("9,") != -1) {
            //            return " <input type='checkbox' class='chk_select'  name='chk'  value='9'" + (data.OperationCheck.indexOf("9,") != -1 ? "checked" : "") + " data-menuid='" + data.MenuID + "'/>"
            //        }
            //    }
            //},
            {
                field: "权限",
                template: function (data) {
                    var check = [];
                    var OperationJSON = JSON.parse(data.OperationJSON);
                    if (!data.IsHaveChildMemu) {
                        check.push("<div style='float:left;margin-right: 15px;'><label class='form-tag checkbox-inline'><input onClick='chkAll(this)' type='checkbox' name='allchk' data-type='" + data.MenuID + "' data-menuid='" + data.MenuID + "'  style='margin-left: -15px;' />全选</label></div>");
                    }
                    if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("0,") != -1) {
                        check.push("<div style='float:left;margin-right: 5px;'><label class='form-tag checkbox-inline'><input  name='chk' type='checkbox' name='chk'  value='0'" + (data.OperationCheck.indexOf("0,") != -1 ? "checked" : "") + " class='" + data.MenuID + "' data-menuid='" + data.MenuID + "' style='margin-left: -15px;' />查看</label></div>");
                    }
                    if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("1,") != -1) {
                        check.push("<div style='float:left;margin-right: 5px;'><label class='form-tag checkbox-inline'><input  name='chk' type='checkbox' value='1'" + (data.OperationCheck.indexOf("1,") != -1 ? "checked" : "") + " class='" + data.MenuID + "' data-menuid='" + data.MenuID + "' style='margin-left: -15px;'/>新增</label></div>");
                    }
                    if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("2,") != -1) {
                        check.push("<div style='float:left;margin-right: 5px;'><label class='form-tag checkbox-inline'><input  name='chk' type='checkbox' value='2'" + (data.OperationCheck.indexOf("2,") != -1 ? "checked" : "") + " class='" + data.MenuID + "' data-menuid='" + data.MenuID + "' style='margin-left: -15px;' />编辑</label></div>");
                    }
                    if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("3,") != -1) {
                        check.push("<div style='float:left;margin-right: 5px;'><label class='form-tag checkbox-inline'><input  name='chk' type='checkbox' value='3'" + (data.OperationCheck.indexOf("3,") != -1 ? "checked" : "") + " class='" + data.MenuID + "' data-menuid='" + data.MenuID + "' style='margin-left: -15px;' />删除</label></div>");
                    }
                    if (!data.IsHaveChildMemu && data.AccessOperation.indexOf("8,") != -1) {
                        check.push("<div style='float:left;margin-right: 5px;'><label class='form-tag checkbox-inline'><input  name='chk' type='checkbox' value='8'" + (data.OperationCheck.indexOf("8,") != -1 ? "checked" : "") + " class='" + data.MenuID + "' data-menuid='" + data.MenuID + "' style='margin-left: -15px;' />导出</label></div>");
                    }
                    $.each(OperationJSON, function (m, n) {
                        check.push("<div style='float:left;'><label class='form-tag checkbox-inline'><input  name='chk' " + (data.OperationCheck.indexOf(+n.ID + ",") != -1 ? "checked" : "") + "  class='" + data.MenuID + "' data-menuid='" + data.MenuID + "' style='margin-left: -15px;' type='checkbox' value='" + n.ID + "'>" + n.OperateName + "</label></div>");
                    })
                    return check.join("");
                }
            }
        ]
    });
}

function chkAll(e) {
    var type = $(e).data("type");
    if ($(e).is(":checked")) {
        $("." + type).iCheck("check");
    } else {
        $("." + type).iCheck("uncheck");
    }
}

function Save() {
    var ops = [];
    $.each($("#grid").find("tbody").find("tr"), function (i, l) {
        if ($(l).hasClass("k-treelist-group")) { return; }
        var obj = {
            RoleID: $("#hid_RoleID").val(),
            MenuID: $(l).find("td").find("input[name=allchk]:eq(0)").data("menuid"),
        }
        var op = [];
        $.each($(l).find("input[name=chk]:checked"), function (m, n) {
            op.push($(n).val());
        })
        obj.Operations = op.join(",");
        ops.push(obj);
    })
    $.post("/AccessRightSetting/PostOperationConfig", { value: JSON.stringify(ops) }, function (data) {
        if (data.Success) {
            dlg.alert("保存成功！", function () {
                parent.dlg.closeAll();
            });
        } else {
            if (data.Message) {
                dlg.alert(data.Message);
            }
        }
    });
    console.log(JSON.stringify(ops));
}

function Close() {
    parent.dlg.closeAll();
}

$(window).resize(function () {
    $("#grid").height($(window).height() - 46);
});
$(function () {
    if ($(window).height() > 0) {
        $("#grid").height($(window).height() - 46);
    } else {
        //$("#grid").height(200);
        //history.go(0)
    }
})