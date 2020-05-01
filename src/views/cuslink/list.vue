<template>
    <div style="position: relative;">
        <Menu tagIndex="7"></Menu>
        <div class="rightMain">
            <Header></Header>
            <div id="list">
                <div class="search form-horizontal" style="padding:10px 20px 0 10px;overflow:auto;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="col-md-3 control-label">类型：</label>
                            <div class="col-md-9">
                                <select v-model="LinkType" class="inputclass">
                                    <option value=""></option>
                                    <option v-for="opt in options" :value="opt.id" :key="opt.id">
                                        {{opt.cat_name}}
                                    </option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label class="col-md-3 control-label">标题：</label>
                            <div class="col-md-9">
                                <input type="text" v-model="Title" class="inputclass" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="col-md-3 control-label">URL：</label>
                            <div class="col-md-9">
                                <input type="text" v-model="Url" class="inputclass" style="width:300px;" />
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
                                            <th width="80">编号</th>
                                            <th width="350">标题</th>
                                            <th>地址</th>
                                            <th width="100">访问</th>
                                            <th width="100">类型</th>
                                            <th width="80">状态</th>
                                            <th width="150">添加时间</th>
                                            <th width="150">更新时间</th>
                                            <th width="150">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="resultTable">
                                        <tr v-for="site in sites" v-bind:key="site.id">
                                            <td>{{site.id}}</td>
                                            <td>{{site.title}}</td>
                                            <td>{{site.url}}</td>
                                            <td align="center">{{site.visited}}</td>
                                            <td align="center">{{site.link_type}}</td>
                                            <td>{{site.status}}</td>
                                            <td>{{site.add_date | formatDate}}</td>
                                            <td>{{site.update_date | formatDate}}</td>
                                            <td align="center">
                                                <a v-on:click="editcuslink('CusLinkEdit', {id: site.id})" class="btn btn-sm btn-outline filter-submit purple">
                                                    <i class="fa fa-edit"></i> 修改
                                                </a>
                                                <a v-on:click="delcuslink(site.id)" class="btn btn-sm btn-outline filter-submit dark" style="margin-right:0;">
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
                options: [],
                sites: [],
                total: 5,
                display: 14,
                current: 1,
                LinkType: '',
                Title: '',
                Url: ''
            };
        },
        components: {
            Header,
            Menu,
            Pager
        },
        created: function () {
            var self = this;
            self.current = parseInt(self.$route.params.pageno);
            self.display = parseInt(self.$route.params.size);
            self.load();
            self.loadCat();
        },
        filters: {
            formatDate(time) {
                var date = new Date(time);
                return formatDate(date, "yyyy-MM-dd hh:mm:ss");
            }
        },
        methods: {
            page_change: function (currentPage) {
                var self = this;
                if(self.current != currentPage) {
                    self.current = currentPage;
                    router.push({ name: 'CusLinkList', params: { size: self.display, pageno: self.current } });
                }

                self.load();
            },
            load: function () {
                var self = this;
                httper.post('/api/cuslink/list/' + self.display + '/' + self.current, {
                    cat_id: self.LinkType,
                    title: self.Title,
                    url: self.Url
                }).then(function (response) {
                    self.sites = response.data.list;
                    self.total = response.data.total;
                });
            },
            loadCat: function () {
                var self = this;
                httper.get('/api/link/cat/list').then(function (response) {
                    self.options = response.data;
                });
            },
            editcuslink: function (name, params) {
                router.push({ name: name, params: params });
            },
            delcuslink: function (id) {
                var self = this;
                if (confirm("确认要删除？")) {
                    httper.get('/api/cuslink/delete/' + id).then(function (response) {
                        if (response.data > 0) {
                            self.load();
                        }
                    });
                }
            },
            search: function () {
                var self = this;
                self.$refs.pager.setSearch(1);
            }
        }
    };
</script>