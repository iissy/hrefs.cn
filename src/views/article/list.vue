<template>
    <div style="position: relative;">
        <Menu tagIndex="5"></Menu>
        <div class="rightMain">
            <Header></Header>
            <div id="list">
                <div class="search form-horizontal" style="padding:10px 20px 0 10px;overflow:auto;">
                    <div class="form-group">
                        <div class="col-md-3">
                            <label class="col-md-2 control-label">编号：</label>
                            <div class="col-md-10">
                                <input type="text" v-model="Id" class="inputclass" style="width:300px;" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="col-md-2 control-label">标题：</label>
                            <div class="col-md-10">
                                <input type="text" v-model="Title" class="inputclass" style="width:300px;" />
                            </div>
                        </div>
                        <div class="col-md-1">
                            <button class="hd-button" v-on:click="search">搜  索</button>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <!-- BEGIN EXAMPLE TABLE PORTLET-->
                        <div class="portlet light">
                            <div class="portlet-body">
                                <table class="table table-striped table-bordered table-hover" id="mytable">
                                    <thead>
                                        <tr>
                                            <th width="400">编号</th>
                                            <th>标题</th>
                                            <th width="100">访问</th>
                                            <th width="150">时间</th>
                                            <th width="150">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="resultTable">
                                        <tr v-for="article in datas" v-bind:key="article.id">
                                            <td style="max-width:340px;overflow:hidden;white-space:nowrap;">{{article.id}}</td>
                                            <td><a :href="'https://www.hrefs.cn/article/'+article.id" target="_blank">{{article.title}}</a></td>
                                            <td align="center">{{article.visited}}</td>
                                            <td>{{article.create_time | formatDate}}</td>
                                            <td align="center">
                                                <a v-on:click="editarticle('ArticleEdit', {id: article.id})" class="btn btn-sm btn-outline filter-submit purple">
                                                    <i class="fa fa-edit"></i> 修改
                                                </a>
                                                <a v-on:click="delarticle(article.id)" class="btn btn-sm btn-outline filter-submit dark" style="margin-right:0;">
                                                    <i class="fa fa-lock"></i> 删除
                                                </a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <Pager ref="pager" :total="total" :current='current' :display='display' @page_change="page_change"></Pager>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Header from '../../components/header';
    import Menu from '../../components/menu';
    import Pager from '../../components/pager';
    import httper from '../../util/httper';
    import { formatDate } from '../../util/date.js';
    import router from '../../router';

    export default {
        data: function () {
            return {
                datas: [],
                total: 5,
                display: 14,
                current: 1,
                Id: '',
                Catalog: '',
                Title: ''
            };
        },
        components: {
            Header,
            Menu,
            Pager
        },
        created: function () {
            let self = this;
            self.current = parseInt(self.$route.params.pageno);
            self.display = parseInt(self.$route.params.size);
            self.load();
        },
        filters: {
            formatDate(time) {
                let date = new Date(time);
                return formatDate(date, "yyyy-MM-dd hh:mm:ss");
            }
        },
        methods: {
            page_change: function (currentPage) {
                let self = this;
                if(self.current != currentPage) {
                    self.current = currentPage;
                    router.push({ name: 'ArticleList', params: { size: self.display, pageno: self.current } });
                }

                self.load();
            },
            load: function () {
                let self = this;
                httper.post('/api/article/list/' + self.display + '/' + self.current, {
                    id: self.Id,
                    title: self.Title
                }).then(function (response) {
                    self.datas = response.data.list;
                    self.total = response.data.total;
                });
            },
            editarticle: function (name, params) {
                router.push({ name: name, params: params });
            },
            delarticle: function (id) {
                var self = this;
                if (confirm("确认要删除？")) {
                    httper.get('/api/article/delete/' + id).then(function (response) {
                        if (response.data > 0) {
                            self.load();
                        }
                    });
                }
            },
            search: function () {
                let self = this;
                self.$refs.pager.setSearch(1);
            }
        }
    };
</script>