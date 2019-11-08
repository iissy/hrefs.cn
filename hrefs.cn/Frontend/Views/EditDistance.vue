<template>
    <div id="main">
        <div style="width:50%;border-left: 1px solid #eeeeee;height:600px;float:left;">
            <div style="overflow:hidden;margin-top:10px;border-bottom: 1px solid #eeeeee;">
                <div class="form-group">
                    <label class="col-md-12 control-label font-green-dark">比较两个图片的差异</label>
                </div>
            </div>
            <div style="overflow:hidden;margin-top:20px;">
                <div class="form-group">
                    <div class="col-md-6 text-center">
                        <img src="/favicon1.png" />
                    </div>
                    <div class="col-md-6 text-center">
                        <img src="/favicon2.png" />
                    </div>
                </div>
            </div>
            <div style="overflow:hidden;margin-top:20px;">
                <div class="form-group">
                    <div class="col-md-12">
                        <button style="color:#ffffff;padding:5px 50px 5px 50px;background-color: #36c6d3;font-size:16px;border-radius:10px;border: 1px solid #2bb8c4;float:left;" type="button" v-on:click="post1">计算两个图片的差异</button>
                    </div>
                </div>
            </div>

            <div style="overflow:hidden;margin-top:100px;">
                <div style="overflow:hidden;border-bottom: 1px solid #eeeeee;">
                    <div class="form-group">
                        <label class="col-md-12 control-label font-green-dark">比较两个字符串的差异</label>
                    </div>
                </div>
                <div style="overflow:hidden;margin-top:20px;">
                    <div class="form-group">
                        <div class="col-md-1">
                            &nbsp;
                        </div>
                        <div class="col-md-5 text-center">
                            <input type="text" v-model="str1" name="str1" class="form-control" placeholder="字符串1">
                        </div>
                        <div class="col-md-5 text-center">
                            <input type="text" v-model="str2" name="str2" class="form-control" placeholder="字符串2">
                        </div>
                        <div class="col-md-1">
                            &nbsp;
                        </div>
                    </div>
                </div>
                <div style="overflow:hidden;margin-top:20px;">
                    <div class="form-group">
                        <div class="col-md-12">
                            <button style="color:#ffffff;padding:5px 50px 5px 50px;background-color: #36c6d3;font-size:16px;border-radius:10px;border: 1px solid #2bb8c4;float:left;" type="button" v-on:click="post2">计算两个字符串的差异</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="width:50%;border-left: 1px solid #eeeeee;height:800px;float:left;">
            <div style="overflow:hidden;margin-top:10px;">
                <div class="form-group">
                    <div class="col-md-12 font-green-dark">
                        <div>显示结果：<span v-text="result"></span></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import httper from '../util/httper';
    import '../util/upload';

    export default {
        data: function () {
            return {
                str1: 'kitten',
                str2: 'sitting',
                result: ''
            };
        },
        methods: {
            post1: function () {
                var self = this;

                httper.get('/editdistance1').then(function (response) {
                    self.result = response.data.result;
                    }).catch(function (error) {
                        console.log(error);
                    });

                if (!!$.trim(self.Icon)) {
                    
                }
            },
            post2: function () {
                var self = this;
                if (!!$.trim(self.str1) && !!$.trim(self.str2)) {
                    httper.post('/editdistance2', {
                        str1: self.str1,
                        str2: self.str2
                }).then(function (response) {
                    self.result = response.data.result;
                    }).catch(function (error) {
                        console.log(error);
                    });
                }
            }
        }
    };
</script>