<template>
    <div style="position: relative;">
        <Menu tagIndex="2"></Menu>
        <div class="rightMain">
            <div style="padding:0 0 0 0;height:60px;margin-bottom:10px;">
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
                                                        <option value=""></option>
                                                        <option v-for="opt in options" :value="opt.id" :key="opt.id">
                                                            {{opt.cat_name}}
                                                        </option>
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
                                                            <input type="file" id="upload" @change="change" name="upload" />
                                                        </span>
                                                    </div>
                                                    <input type="text" v-model="Icon" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label">导航简介</label>
                                                <div class="col-md-11">
                                                    <input type="text" class="form-control" v-model="Brief" />
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
    import router from '../../router';
    import httper from '../../util/httper';
    import server from "../../conf/config";
    import $ from 'jquery';

    export default {
        data: function () {
            return {
                options: [],
                LinkType: '',
                Url: '',
                Title: '',
                Brief: '',
                Icon: '',
                Id: ''
            };
        },
        components: {
            Menu
        },
        created: function () {
            let self = this;
            self.loadCat();
            if (self.$route.params.id) {
                let url = '/api/link/get/' + self.$route.params.id;
                httper.get(url).then(function (response) {
                    self.Id = response.data.id;
                    self.Title = response.data.title;
                    self.Icon = response.data.icon;
                    self.Brief = response.data.brief;
                    self.Url = response.data.url;
                    self.LinkType = response.data.cat_id;
                });
            }
        },
        methods: {
            change: function() {
                let self = this;
                $("#upload").upload(server.upload, function (response) {
                    self.Icon = response.url;
                });
            },
            loadCat: function () {
                let self = this;
                httper.get('/api/link/cat/list').then(function (response) {
                    self.options = response.data;
                });
            },
            post: function () {
                let self = this;
                if (!!$.trim(self.Brief) && !!$.trim(self.Title) && !!$.trim(self.Url) && !!$.trim(self.LinkType)) {
                    httper.post('/api/link/save', {
                        id: self.Id,
                        cat_id: self.LinkType,
                        title: self.Title,
                        icon: self.Icon,
                        brief: self.Brief,
                        url: self.Url
                    }).then(function (response) {
                        if (response.data > 0) {
                            router.push({ name: 'LinkList', params: { size: 14, pageno: 1 }});
                        }
                    });
                }
            }
        }
    };
</script>