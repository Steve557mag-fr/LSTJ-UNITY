mergeInto(LibraryManager.library, {
    setCookie :function(name, data){
        let str_name = UTF8ToString(name);
        let str_data = UTF8ToString(data);
        let cookie_arr = {};
        
        try{
            cookie_arr = JSON.parse(raw_cookies);
        }
        catch{
            cookie_arr = {};
        }
        
        cookie_arr[str_name] = str_data;
        document.cookie = JSON.stringify(cookie_arr);
        console.log("cookie set!");

    },
    getCookie :function (name) {
        let name_str = UTF8ToString(name);
        let cookie_arr = {};
        try{

            console.log("reading cookies...");
            cookie_arr = JSON.parse(raw_cookies);
            
            let ret = cookie_arr[name_str];
            console.log("cookie found : ", ret);
            let ptr_size = lengthBytesUTF8(ret) + 1; 
            let new_ptr  = _malloc(ptr_size);
            stringToUTF8(ret, new_ptr, ptr_size);
            return new_ptr; 

        }
        catch{
            return;
        }


    }
});
