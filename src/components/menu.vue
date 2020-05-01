<template>
    <div class="leftBar">
        <div id="header">
            <div style="margin:0 auto 0 auto;text-align:left;height:40px;">
                <div style="overflow:hidden;">
                    <div style="margin-left:30px;margin-top: 10px;float:left;">
                        <a href="/" class="name">爱斯园</a>
                    </div>
                    <div style="width: 18px;height: 18px;float: right;margin-top: 11px;margin-right: 10px;">
                        <img src="/shouqi.png" />
                    </div>
                </div>
            </div>
        </div>
        <ul>
            <li :class="{active: (currentIndex == 1)}"><router-link to="/main">个人首页</router-link></li>
            <li><span></span></li>
            <li :class="{active: (currentIndex == 2)}"><router-link to="/main/link/add">添加导航</router-link></li>
            <li :class="{active: (currentIndex == 3)}"><router-link to="/main/link/14/1">导航管理</router-link></li>
            <li><span></span></li>
            <li :class="{active: (currentIndex == 4)}"><router-link to="/main/article/add">添加文章</router-link></li>
            <li :class="{active: (currentIndex == 5)}"><router-link to="/main/article/14/1">文章管理</router-link></li>
            <li><span></span></li>
            <li :class="{active: (currentIndex == 6)}"><router-link to="/main/cuslink/add">添加网摘</router-link></li>
            <li :class="{active: (currentIndex == 7)}"><router-link to="/main/cuslink/14/1">网摘管理</router-link></li>
            <li><span></span></li>
            <li :class="{active: (currentIndex == 8)}"><router-link to="/main/account/14/1">用户列表</router-link></li>
        </ul>
    </div>
</template>

<script>
    export default {
        data: function () {
            return {
                currentIndex: parseInt(this.tagIndex)
            };
        },
        props: ['tagIndex']
    };
</script>