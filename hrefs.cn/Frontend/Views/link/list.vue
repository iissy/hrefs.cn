<template>
    <div style="position: relative;">
        <Menu tagIndex="3"></Menu>
        <div class="rightMain">
            <div style="padding:0 0 0 0;height:60px;margin-bottom:20px;">
                <div style="background-color: #ffffff;height:60px;padding:10px;"></div>
            </div>
            <div id="list">
                <div class="search form-horizontal" style="padding:10px 20px 0 10px;overflow:auto;">
                    <div class="form-group">
                        <div class="col-md-2">
                            <label class="col-md-3 control-label">类型：</label>
                            <div class="col-md-9">
                                <select v-model="LinkType" class="inputclass">
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
                                            <th width="80">小图标</th>
                                            <th width="200">标题</th>
                                            <th>地址</th>
                                            <th width="100">访问</th>
                                            <th width="100">类型</th>
                                            <th width="150">时间</th>
                                            <th width="150">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="resultTable">
                                        <tr v-for="site in sites">
                                            <td class="show-img"><img v-if="site.icon" :src="site.icon" style="height:20px;width:20px;" />&nbsp;</td>
                                            <td>{{site.title}}</td>
                                            <td>{{site.url}}</td>
                                            <td align="center">{{site.visited}}</td>
                                            <td align="center">{{site.linkType}}</td>
                                            <td>{{site.createTime | formatDate}}</td>
                                            <td align="center">
                                                <a v-on:click="editlink('LinkEdit', {id: site.id})" class="btn btn-sm btn-outline filter-submit purple">
                                                    <i class="fa fa-edit"></i> 修改
                                                </a>
                                                <a v-on:click="dellink(site.id)" class="btn btn-sm btn-outline filter-submit dark" style="margin-right:0;">
                                                    <i class="fa fa-lock"></i> 删除
                                                </a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <Pager :total="total" :current='current' :display='display' @pagechange="pagechange"></Pager>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Menu from '../../components/menu';
    import Pager from '../../components/pager';
    import httper from '../../util/httper';
    import { formatDate } from '../../util/date.js';
    import router from '../../router';

    export default {
        data: function () {
            return {
                sites: [],
                total: 5,
                display: 15,
                current: 1,
                LinkType: '',
                Title: '',
                Url: ''
            };
        },
        components: {
            Menu,
            Pager
        },
        created: function () {
            var self = this;
            self.current = parseInt(self.$route.params.pageno);
            self.display = parseInt(self.$route.params.size);
            self.load();
        },
        filters: {
            formatDate(time) {
                var date = new Date(time);
                return formatDate(date, "yyyy-MM-dd hh:mm:ss");
            }
        },
        methods: {
            pagechange: function (currentPage) {
                var self = this;
                self.current = currentPage;
                self.load();
            },
            load: function () {
                var self = this;
                httper.post('/link/list/' + self.display + '/' + self.current, {
                    linktype: self.LinkType,
                    title: self.Title,
                    url: self.Url
                }).then(function (response) {
                    self.sites = response.data.list;
                    self.total = response.data.total;
                }).catch(function (error) {
                    console.log(error);
                });
            },
            editlink: function (name, params) {
                router.push({ name: name, params: params });
            },
            dellink: function (id) {
                var self = this;
                if (confirm("确认要删除？")) {
                    httper.get('/link/delete/' + id).then(function (response) {
                        if (response.data.result === 1) {
                            self.load();
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            },
            search: function () {
                var self = this;
                self.pagechange(1);
            }
        }
    };
</script>