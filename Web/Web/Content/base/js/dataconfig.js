$(document).ready(function () {
    Seach();
});

function Seach() {
    $("#grid").kendoGrid(
        {
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: "/AccessRightSetting/_DataConfigList?roleid=" + $("#hid_RoleID").val(),
                        type: "POST",
                        dataType: "json",
                    }
                },
                schema: {
                    data: "List",
                    total: "Total",
                    errors: "Errors",
                },
                error: function (e) {
                    //  display_kendoui_grid_error(e);
                    console.log(JSON.stringify(e));
                    // Cancel the changes
                    this.cancelChanges();
                },
            },
            columns: [
             { width: 250, field: "ShowName", expandable: true, title: "实体名称" },
             {
                 width: 100,
                 title: "查看范围",
                 headerAttributes: { style: "text-align:center" },
                 attributes: { style: "text-align:center" },
                 template: function (data) {
                     return GetColor(data.ViewRight, data.EntityID)
                 }
             },
            {
                width: 100,
                title: "编辑范围",
                headerAttributes: { style: "text-align:center" },
                attributes: { style: "text-align:center" },
                hidden: true,
                template: function (data) {
                    return GetColor(data.EditRight, data.EntityID)
                }
            },
            //{
            //    width: 100,
            //    title: "删除范围",
            //    headerAttributes: { style: "text-align:center" },
            //    attributes: { style: "text-align:center" },
            //    template: function (data) {
            //        return GetColor(data.DeleteRight, data.EntityID)
            //    }
            //},
            {
                title: "",
            },
            ]
        });
}

function Save() {
    var ops = [];
    $.each($("#grid").find("tbody").find("tr"), function (i, l) {
        var obj = {
            RoleID: $("#hid_RoleID").val(),
            EntityID: $(l).find("td").find("span").data("entityid"),
            ViewRight: $(l).find("td:eq(1)").find("span").data("value"),
            EditRight: $(l).find("td:eq(2)").find("span").data("value"),
            DeleteRight: $(l).find("td:eq(3)").find("span").data("value"),
        }
        ops.push(obj);
    })

    $.post("/AccessRightSetting/PostDataConfig", { value: JSON.stringify(ops) }, function (data) {
        if (data.Success) {
            dlg.alert("保存成功！");
        } else {
            if (data.Message) {
                dlg.alert(data.Message);
            }
        }
    });
}

function GetColor(value, entityid) {
    var cla = " badge-warning";
    var txt = "个人";
    switch (value) {
        case 0:
            //    cla = " badge-success"; txt = "无权限";
            //    break;
        case 1:
            cla = " badge-success"; txt = "个人";
            break;
        case 2:
            cla = " badge-info"; txt = "部门";
            break;
            //case 3:
            //    cla = " badge-success"; txt = "上下级部门";
            //    break;
        case 4:
            cla = " badge-inverse"; txt = "全部";
            break;
        default:
            value = 1;
            break;
    }
    return '<span onclick="changeColor(this)" data-entityid="' + entityid + '" data-value="' + value + '" class="badge ' + cla + '">' + txt + '</span>'
}

function changeColor(e) {
    if ($(e).hasClass("badge-success")) {
        $(e).removeClass("badge-success").addClass("badge-info").html("部门").data("value", 2);
    } else if ($(e).hasClass("badge-info")) {
        $(e).removeClass("badge-info").addClass("badge-inverse").html("全部").data("value", 4);
    } else if ($(e).hasClass("badge-inverse")) {
        $(e).removeClass("badge-inverse").addClass("badge-success").html("个人").data("value", 1);
    } //else if ($(e).hasClass("badge-warning")) {
    //    $(e).removeClass("badge-warning").addClass("badge-success").html("无权限").data("value", 0);
    //}

}

function autogridsize() {
    $("#grid").height($(window).height() - 46);
    Seach();
}

$(window).resize(function () {
    $("#treelist").height($(window).height() - 46);
    autogridsize();

});
$(function () {
    autogridsize();
    $("#treelist").height($(window).height() - 46);
})