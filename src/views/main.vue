<template>
    <div>
        <Menu tagIndex="1"></Menu>
        <div class="rightMain">
            <Header></Header>
            <div id="list">
                <div style="margin:10px;">主题</div>
            </div>
        </div>
    </div>
</template>

<script>
    import Header from '../components/header';
    import Menu from '../components/menu';

    export default {
        data: function () {
            return {
            }
        },
        components:{
            Header,
            Menu
        }
    };
</script>