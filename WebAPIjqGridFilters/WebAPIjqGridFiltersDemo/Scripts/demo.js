$(function () {
    $("#companies").jqGrid({
        url: 'api/Companies',
        datatype: "json",
        colNames: ['id', 'Name', 'Address'],
        colModel: [
            { name: 'Name', index: 'Name', width: 200 },
            { name: 'Address', index: 'Address', width: 300 }
        ],
        rowNum: 2,
        rowList: [2, 4, 8],
        pager: '#companiesPager',
        sortname: 'id',
        viewrecords: true,
        sortorder: "desc",
        caption: "Companies",
    });
    jQuery("#companies").jqGrid('navGrid', '#companiesPager');

})