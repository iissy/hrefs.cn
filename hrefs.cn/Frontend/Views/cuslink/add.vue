<template>
    <div style="position: relative;">
        <Menu tagIndex="6"></Menu>
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
                                                        <option value=""></option>
                                                        <option v-for="opt in options" :value="opt.id" :key="opt.id">
                                                            {{opt.catname}}
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

    export default {
        data: function () {
            return {
                options: [],
                LinkType: '',
                Url: '',
                Title: '',
                Id: '',
                showModuleName: false
            };
        },
        components: {
            Menu
        },
        created: function () {
            var self = this;
            self.loadCat();
            if (self.$route.params.id) {
                var url = '/cuslink/get/' + self.$route.params.id;
                httper.get(url).then(function (response) {
                    self.Id = response.data.id;
                    self.Title = response.data.title;
                    self.Url = response.data.url;
                    self.LinkType = response.data.catid;
                }).catch(function (error) {
                    console.log(error);
                });
            }
        },
        methods: {
            loadCat: function () {
                var self = this;
                httper.get('/link/cat/list').then(function (response) {
                    self.options = response.data;
                }).catch(function (error) {
                    console.log(error);
                });
            },
            post: function () {
                var self = this;
                if (!!$.trim(self.Title) && !!$.trim(self.Url) && !!$.trim(self.LinkType)) {
                    httper.post('/cuslink/save', {
                        Id: self.Id,
                        Catid: self.LinkType,
                        Title: self.Title,
                        Url: self.Url
                    }).then(function (response) {
                        if (response.data.result === 1) {
                            router.push({ name: 'CusLinkList', params: { size: 15, pageno: 1 } });
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            }
        }
    };
</script>