var attachment = {
    init: function (options) {
        var _option = {
            FileExt: "*.jpg;*.jpeg;*.gif;*.png;*.doc;*.xls;*.ppt;*.txt;*.zip;*.rar;*.pdf;",
            FileSize: 10
        }
        $.extend(_option, options || {});
        var attachfields = [];
        layoutform.fn_beforeSendArr.push(function () {
            $.each(attachfields, function (m, n) {
                $.post("/Attachment/Enable", { json: JSON.stringify(n.AttachmentList) }, function (data) { });
            })
        });
        $(".filesupload").each(function (m, n) {
            var field = $(n).data("id");

            var attachfield = {
                ID: field,
                AttachmentList: []
            }
            $.each($("#" + field).parent("div").find(".uploadifyQueueDB>div"), function (i, db) {
                attachfield.AttachmentList.push({
                    ID: $(db).data("id"),
                    Name: $(db).data("name"),
                    Attachment: $(db).data("attachment"),
                    Size: $(db).data("size"),
                });
            });
            attachfields.push(attachfield);


            var attachmentlist = [];
            var _options = {
                uploader: "/Content/plugin/uploadify/uploadify.swf",
                script: "/Attachment/AjaxAttachmentUpload",
                cancelImg: "/Content/plugin/uploadify/cancel.png",
                buttonImg: "/Content/plugin/uploadify/selectfile.png",
                folder: "/UploadFile/",
                auto: false,
                multi: true,
                width: 140,
                removeCompleted: false,
                //height: 150,
                fileExt: _option.FileExt,
                fileDesc: '文件',
                queueSizeLimit: 10,
                sizeLimit: 1024 * parseInt(_option.FileSize || 10) * 1024, // 200 kB = 200 * 1024 bytes
                // 多个参数用逗号隔开 'name':$('#name').val(),'num':$('#num').val(),'ttl':$('#ttl').val()
                onSelectOnce: function (event, data) {
                    $(n).uploadifyUpload();
                },
                onError: function (event, queueId, fileObj, errorObj)	//错误提示
                {

                },
                onAllComplete: function (e, data) {
                    $("#" + field).val(JSON.stringify(attachfield.AttachmentList).replace("[]", ""));
                    //如果是修改页面
                    if (layoutform.isedit == "True") {
                        //修改当前数据的附件值
                        $.post(layoutform.path + "UpdateAttachmentById", { fieldname: field, value: JSON.stringify(attachfield.AttachmentList), id: layoutform.ID }, function (data) { });
                    }
                },
                onComplete: function (event, queueID, fileObj, response, data) {
                    var res = JSON.parse(response);
                    if (res.Success) {
                        attachfield.AttachmentList.push({
                            ID: res.Item.ID,
                            Name: res.Item.Name,
                            Attachment: res.Item.Attachment,
                            Size: res.Item.Size,
                            QueueID: queueID
                        });
                    } else {
                        dlg.msg.info(res.Message);
                        $("#file_" + attachfield.ID + "" + queueID).remove();
                        return false;
                    }
                    layoutform.resize();
                },
                onCancel: function (event, ID, fileObj, data, clearFast) {
                    var file = attachfield.AttachmentList.getobj("QueueID", ID);
                    if (file != null) {
                        if (confirm("确定删除附件 “" + file.Name + "” 吗？")) {
                            attachfield.AttachmentList.removeobj("QueueID", ID);
                            $.post("/Attachment/DeleteFileAttachment", { id: file.ID }, function (data) { });
                            //修改当前数据的附件值
                            $.post(layoutform.path + "UpdateAttachmentById", { fieldname: field, value: JSON.stringify(attachfield.AttachmentList).replace("[]", ""), id: layoutform.ID }, function (data) { });
                        } else {
                            return false;
                        }
                    } else {
                        file = attachfield.AttachmentList.getobj("ID", ID);
                        if (file != null) {
                            if (confirm("确定删除附件 “" + file.Name + "” 吗？")) {
                                var index = layer.load(2);
                                //console.log(JSON.stringify(attachfield))
                                attachfield.AttachmentList.removeobj("ID", ID);
                                //修改当前数据的附件值
                                $.post(layoutform.path + "UpdateAttachmentById", { fieldname: field, value: JSON.stringify(attachfield.AttachmentList).replace("[]", ""), id: layoutform.ID }, function (data) {
                                    layer.close(index);
                                });
                                $.post("/Attachment/DeleteFileAttachment", { id: ID }, function (data) { });
                            } else {
                                return false;
                            }
                        }

                    }
                    $("#" + field).val(JSON.stringify(attachfield.AttachmentList).replace("[]", ""));
                },
                //onOpen: function () {
                //    alert("");
                //},
                'onUploadError': function (file, errorCode, errorMsg, errorString) {
                    alert('The file ' + file.name + ' could not be uploaded: ' + errorString);
                },
                'onSelectError': function (file, errorCode, errorMsg) {
                    debugger
                    switch (errorCode) {
                        case -100:
                            alert("上传的文件数量已经超出系统限制的" + $('#uploadify1').uploadify('settings', 'queueSizeLimit') + "个文件！");
                            break;
                        case -110:
                            alert("图像大小超限，请调低像素后重新上传 ");
                            break;
                        case -120:
                            alert("文件 [" + file.name + "] 大小异常！");
                            break;
                        case -130:
                            alert("文件 [" + file.name + "] 类型不正确！");
                            break;
                    }
                },
            }

            $(n).uploadify(_options);
        })
    }
}