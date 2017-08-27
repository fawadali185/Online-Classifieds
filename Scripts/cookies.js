function save(name, value) { 
    $.cookie(name, value, { path: '/', expires: 365 });
    
}