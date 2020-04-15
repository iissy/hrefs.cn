import axios from 'axios'
import server from "../conf/config";

let httper = {
    get(url, data) {
        return new Promise((resolve, reject) => {
            axios.get(server.url + url, {
                params: data
            }).then((response) => {
                if (response) {
                    resolve(response);
                }
            }).catch((error) => {
                reject(error);
            })
        })
    },

    post(url, data) {
        return new Promise((resolve, reject) => {
            axios.post(server.url + url, data, {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'Accept': 'application/json'
                }
            }).then((response) => {
                if (response) {
                    resolve(response);
                }
            }).catch((error) => {
                reject(error);
            })
        })
    }
};

export default httper;