/**
 * Theme: Dastyle - Responsive Bootstrap 4 Admin Dashboard
 * Author: Mannatthemes
 * Form Wizard
 */

$(function () {
    //$("#form-horizontal").steps({
    //    headerTag: "h3",
    //    bodyTag: "fieldset",
    //    transitionEffect: "slide"
    //});
    $("#productForm").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slideLeft",
        stepsOrientation: "vertical",
        labels:
        {
            finish: "Bitir",
            next: "Ileri",
            previous:"Geri"
        }
    }); 
    $("#mailWizard").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slide",
        forceMoveForward: true,
        enablePagination: false,
    });
    $("#sellerForm").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slide",
        forceMoveForward: true,
        enablePagination: false,
    });
    $("#phoneWizard").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slide",
        forceMoveForward: true,
        enablePagination: false,
    });
});

