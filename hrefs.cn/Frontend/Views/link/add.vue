<template>
    <div style="position: relative;">
        <Menu tagIndex="2"></Menu>
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
                                                <label class="col-md-1 control-label">类型</label>
                                                <div class="col-md-11">
                                                    <select v-model="LinkType" class="form-control">
                                                        <optgroup label="热门">
                                                            <option value="公共">公共</option>
                                                            <option value="资讯">资讯</option>
                                                            <option value="实用工具">实用工具</option>
                                                            <option value="区块链">区块链</option>
                                                            <option value="大数据">大数据</option>
                                                            <option value="人工智能">人工智能</option>
                                                            <option value="架构师">架构师</option>
                                                            <option value="教程">教程</option>
                                                            <option value="数据库">数据库</option>
                                                            <option value="运维工具">运维工具</option>
                                                            <option value="软件">软件</option>
                                                            <option value="协同工具">协同工具</option>
                                                        </optgroup>
                                                        <optgroup label="后端">
                                                            <option value="DotNet">DotNet</option>
                                                            <option value="Golang">Golang</option>
                                                            <option value="NodeJS">NodeJS</option>
                                                            <option value="Python">Python</option>
                                                            <option value="Java">Java</option>
                                                            <option value="PHP">PHP</option>
                                                            <option value="C/C++">C/C++</option>
                                                            <option value="Ruby">Ruby</option>
                                                            <option value="其他">其他</option>
                                                        </optgroup>
                                                        <optgroup label="前端">
                                                            <option value="CSS">CSS</option>
                                                            <option value="JQuery">JQuery</option>
                                                            <option value="Charts">Charts</option>
                                                            <option value="Vue">Vue</option>
                                                            <option value="富文本编辑器">富文本编辑器</option>
                                                            <option value="前端框架">前端框架</option>
                                                            <option value="打包构建">打包构建</option>
                                                        </optgroup>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">标题</label>
                                                <div class="col-md-11">
                                                    <input type="text" v-model="Title" class="form-control" placeholder="请在此填写标题" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">导航</label>
                                                <div class="col-md-11">
                                                    <input type="text" v-model="Url" class="form-control" placeholder="请在此填写url地址" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">小图标</label>
                                                <div class="col-md-11">
                                                    <div id="imgfileinput" data-provides="fileinput" class="fileinput fileinput-new right">
                                                        <span class="btn green btn-file">
                                                            <span class="fileinput-new"> 选择小图标 </span>
                                                            <input type="file" id="file" @change="change" name="image" />
                                                        </span>
                                                    </div>
                                                    <input type="text" v-model="Icon" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">导航简介</label>
                                                <div class="col-md-11">
                                                    <vue-html5-editor :content="Brief" :height="528" :auto-height="false" :show-module-name="showModuleName" @change="updateData" ref="editor"></vue-html5-editor>
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
    import VueHtml5Editor from 'vue-html5-editor';
    import router from '../../router';
    import httper from '../../util/httper';
    import '../../util/upload';

    Vue.use(VueHtml5Editor, {
        showModuleName: true,
        image: {
            sizeLimit: 512 * 1024,
            upload: {
                url: '/upload'
            }
        }
    });

    export default {
        data: function () {
            return {
                LinkType: '',
                Url: '',
                Title: '',
                Brief: '',
                Icon: '',
                Id: '',
                showModuleName: false
            };
        },
        components: {
            Menu
        },
        created: function () {
            var self = this;
            if (self.$route.params.id) {
                var url = '/link/get/' + self.$route.params.id;
                httper.get(url).then(function (response) {
                    self.Id = response.data.id;
                    self.Title = response.data.title;
                    self.Icon = response.data.icon;
                    self.Brief = response.data.brief;
                    self.Url = response.data.url;
                    self.LinkType = response.data.linkType;
                }).catch(function (error) {
                    console.log(error);
                });
            }
        },
        methods: {
            updateData: function (data) {
                this.Brief = data;
            },
            fullScreen: function () {
                this.$refs.editor.enableFullScreen();
            },
            focus: function () {
                this.$refs.editor.focus();
            },
            change: function () {
                var self = this;
                $("#file").upload('/upload', function (response) {
                    self.Icon = response.data;
                });
            },
            post: function () {
                var self = this;
                if (!!$.trim(self.Brief) && !!$.trim(self.Title) && !!$.trim(self.Url) && !!$.trim(self.LinkType)) {
                    httper.post('/link/save', {
                        Id: self.Id,
                        LinkType: self.LinkType,
                        Title: self.Title,
                        Icon: self.Icon,
                        Brief: self.Brief,
                        Url: self.Url
                    }).then(function (response) {
                        if (response.data.result === 1) {
                            router.push({ name: 'LinkList', params: { size: 15, pageno: 1 } });
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            }
        }
    };
</script>