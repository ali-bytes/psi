function showHidThree(one,two,three,t1,t2,t3,ctrlVal) {
    switch (ctrlVal) {
        case t1:
            $(one).fadeIn();
            $(two).hide();
            $(three).hide();
            break;
        case t2:
            $(one).hide();
            $(two).fadeIn();
            $(three).hide();
            break;
        case t3:
            $(one).hide();
            $(two).hide();
            $(three).fadeIn();
            break;

        default:
            $(one).hide();
            $(two).hide();
            $(three).hide();
    }
}