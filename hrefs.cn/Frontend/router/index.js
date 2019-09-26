import Vue from 'vue';
import Router from 'vue-router';

import Main from '../views/main';
import LinkAdd from '../views/link/add';
import LinkList from '../views/link/list';
import ArticleAdd from '../views/article/add';
import ArticleList from '../views/article/list';
import AccountList from '../views/account/list';
import CusLinkList from '../views/cuslink/list';

Vue.use(Router);

const routes = [
    {
        path: '/',
        redirect: '/main'
    }, {
        path: '/main',
        meta: { title: "首页" },
        component: Main
    }, {
        path: '/main/link/add',
        meta: { title: "添加导航" },
        component: LinkAdd
    }, {
        path: '/main/link/edit/:id',
        meta: { title: "编辑导航" },
        name: 'LinkEdit',
        component: LinkAdd
    }, {
        path: '/main/link/:size/:pageno',
        meta: { title: "导航管理" },
        name: 'LinkList',
        component: LinkList
    }, {
        path: '/main/article/add',
        meta: { title: "添加文章" },
        component: ArticleAdd
    }, {
        path: '/main/article/edit/:id',
        meta: { title: "编辑文章" },
        name: 'ArticleEdit',
        component: ArticleAdd
    }, {
        path: '/main/article/:size/:pageno',
        meta: { title: "文章管理" },
        name: 'ArticleList',
        component: ArticleList
    },{
        path: '/main/account/:size/:pageno',
        meta: { title: "用户列表" },
        name: 'AccountList',
        component: AccountList
    }, {
        path: '/main/cuslink/:size/:pageno',
        meta: { title: "用户提交" },
        name: 'CusLinkList',
        component: CusLinkList
    }
];

export default new Router({
    mode: 'history',
    routes: routes,
    scrollBehavior(to, from, savedPosition) {
        return { x: 0, y: 0 };
    }
});