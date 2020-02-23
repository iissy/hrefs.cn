<template>
    <div style="position: relative;">
        <Menu tagIndex="4"></Menu>
        <div class="rightMain">
            <div style="padding:0 0 0 0;height:60px;margin-bottom:20px;">
                <div style="background-color: #ffffff;height:60px;padding:10px;"></div>
            </div>
            <div id="list">
                <form class="form-horizontal mg0">
                    <div id="dataform" class="row">
                        <div class="col-md-12">
                            <div class="portlet light" style="margin-bottom: 0;">
                                <div class="portlet-body form" style="margin:0 auto;">
                                    <div class="form-horizontal mg0" role="form">
                                        <div class="form-body" style="padding:0 0 0 0;">
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">标题</label>
                                                <div class="col-md-11">
                                                    <input type="text" v-model="Title" class="form-control" placeholder="请在此填写标题" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">连接</label>
                                                <div class="col-md-11">
                                                    <input type="text" id="Id" v-model="Id" class="form-control" placeholder="请在此填写url链接" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">图片</label>
                                                <div class="col-md-11">
                                                    <div id="imgfileinput" data-provides="fileinput" class="fileinput fileinput-new right">
                                                        <span class="btn green btn-file">
                                                            <span class="fileinput-new"> 选择图片 </span>
                                                            <input type="file" id="upload" @change="change" name="upload" />
                                                        </span>
                                                    </div>
                                                    <input type="text" v-model="Icon" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">简介</label>
                                                <div class="col-md-11">
                                                    <input type="text" class="form-control" v-model="Brief" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">文章内容</label>
                                                <div class="col-md-11">
                                                    <ckeditor v-model="Body" :config="editorConfig"></ckeditor>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label"></label>
                                                <div class="col-md-11">
                                                    <button style="color:#ffffff;padding:5px 50px 5px 50px;background-color: #36c6d3;font-size:16px;border-radius:10px;border: 1px solid #2bb8c4;float:right;" type="button" v-on:click="post">发 布</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    import Menu from '../../components/menu';
    import Vue from 'vue';
    import CKEditor from 'ckeditor4-vue';
    import router from '../../router';
    import httper from '../../util/httper';
    import '../../util/upload';
    import server from "../../conf/config";
    import $ from 'jquery'

    Vue.use(CKEditor);
    export default {
        data: function () {
            return {
                Id: '',
                Title: '',
                Icon: '',
                Brief: '',
                Body: '',
                editorConfig: {
                    toolbarGroups: [
                        {name: 'document', groups: ['mode', 'document', 'doctools']},
                        {name: 'basicstyles', groups: ['basicstyles', 'cleanup']},
                        {name: 'styles', groups: ['styles']},
                        {name: 'links', groups: ['links']},
                        {name: 'insert', groups: ['insert']},
                        {name: 'colors', groups: ['colors']},
                        {name: 'paragraph', groups: ['list', 'blocks', 'bidi', 'align', 'indent', 'paragraph']},
                        {name: 'clipboard', groups: ['clipboard', 'undo']},
                        {name: 'tools', groups: ['tools']}
                    ],
                    height: 500,
                    pasteFilter: false,
                    allowedContent: true,
                    enterMode: 3,
                    shiftEnterMode: 2,
                    filebrowserImageUploadUrl: server.upload,
                    filebrowserUploadUrl: server.upload,
                    // 使上传图片弹窗出现对应的“上传”tab标签
                    removeDialogTabs: 'image:advanced;link:advanced',
                    //粘贴图片时用得到
                    extraPlugins: 'uploadimage,colorbutton',
                    uploadUrl: server.upload
                }
            };
        },
        components: {
            Menu,
            ckeditor: CKEditor.component
        },
        created: function () {
            let self = this;
            if (self.$route.params.id) {
                let url = '/api/article/get/' + self.$route.params.id;
                httper.get(url).then(function (response) {
                    self.Id = response.data.id;
                    self.Title = response.data.title;
                    self.Icon = response.data.icon;
                    self.Brief = response.data.brief;
                    self.Body = response.data.body;
                    $("#Id").attr("disabled", true);
                });
            }
        },
        methods: {
            change: function () {
                let self = this;
                $("#upload").upload(server.upload, function (response) {
                    self.Icon = response.url;
                });
            },
            post: function () {
                var self = this;
                if (!!$.trim(self.Icon) && !!$.trim(self.Id) && !!$.trim(self.Title) && !!$.trim(self.Body) && !!$.trim(self.Brief)) {
                    httper.post('/api/article/save', {
                        id: self.Id,
                        title: self.Title,
                        icon: self.Icon,
                        brief: self.Brief,
                        body: self.Body
                    }).then(function (response) {
                        if (response.data > 0) {
                            router.push({ name: 'ArticleList', params: { size: 14, pageno: 1 } });
                        }
                    });
                }
            }
        }
    };
</script>