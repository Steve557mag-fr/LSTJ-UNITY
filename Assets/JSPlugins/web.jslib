mergeInto(LibraryManager.library, {
    sendMessage: function(message){
        console.log("[PLUGIN]: ", UTF8ToString(message));
        
    },
    setCookie :function(name, data){
        let str_name = UTF8ToString(name);
        let str_data = UTF8ToString(data);
        document.cookie = `${str_name}:${str_data}`;
        console.log("cookie set!");

    },
    getCookie :function (name) {
        console.log("reading cookies...");
        let parts = document.cookie.split(`:`);
        if (parts.length === 2) {
            let ret = parts[1]; // is the cookie-value
            console.log("cookie found : ", ret);

            let ptr_size = lengthBytesUTF8(ret) + 1; 
            let new_ptr  = _malloc(ptr_size);
            stringToUTF8(ret, new_ptr, ptr_size);
            return new_ptr; 
        };
    }
});
