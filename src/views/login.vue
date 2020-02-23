<template>
    <div id="main">
        <div id="app">
            <div id="dataformuser" class="row">
                <div class="col-md-12">
                    <div class="portlet light" style="margin-bottom: 0;">
                        <div class="portlet-body form" style="max-width: 1200px;margin:0 auto;">
                            <div class="form-horizontal mg0" role="form">
                                <div class="form-body" style="padding:0 0 0 0;">
                                    <div class="form-group">
                                        <label class="col-md-4 control-label">账 号</label>
                                        <div class="col-md-4">
                                            <input type="text" v-model="UID" name="UserId" class="form-control" placeholder="账号">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4 control-label">密 码</label>
                                        <div class="col-md-4">
                                            <input type="password" v-model="PWD" name="Password" class="form-control" placeholder="密码">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4 control-label"></label>
                                        <div class="col-md-4">
                                            <button style="color:#ffffff;padding:5px 30px 5px 30px;background-color: #36c6d3;font-size:16px;border-radius:10px;border: 1px solid #2bb8c4;" class="right" v-on:click="login">登 陆</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import httper from '../util/httper';
    import router from '../router';
    import $ from 'jquery'

    export default {
        data: function () {
            return {
                UID: '',
                PWD: ''
            }
        },
        methods: {
            login: function () {
                var self = this;
                if (!!$.trim(self.UID) && !!$.trim(self.PWD)) {
                    let self = this;
                    httper.post('/login', {
                        userid: self.UID,
                        password: self.PWD
                    }).then(function (response) {
                        if (response.data.id > 0) {
                            router.push({ name: 'Main' });
                        }
                    });
                }
            }
        }
    }
</script>