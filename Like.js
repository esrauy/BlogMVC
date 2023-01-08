
    $(function () {
        //sayfa yüklendikten sonra ilk olarak data-note-id attribute'ne sahip olan tüm elementleri ggetirmem gerekecek.
        var noteids = [];   //boş bir dii tanımladım.
    //div olan ve data-note-id attribute'ü olan elementleri seçiyorum. ("div[data-note-id]")
    //each fonkksiyonu ile de her bir elementi geziyorum.
    $("div[data-note-id]").each(function (i, e) {
        //push metodu ile elde ettiğim verileri noteids isimli diziye elkliyorum. $(e). ile elementi seçiyorum. data ile de note-id'li veriye ulaşmmış oluyorum.
        noteids.push($(e).data("note-id"));
            //console.log(noteids);
        });
    $.ajax({
        method: "POST",
    url: "/Note/GetLiked",
    data: {ids: noteids }
        }).done(function (data) { //action'dan geriye bir veri gelmesi gerekiyor. gelen veri sisteme login olan user'ın beğendiği notların listesi olacak. data ile bu listeyi uradan alacağım.
            if (data.result != null && data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    var id = data.result[i];  //beğenmiş olduğum notun id'sini almış oldum.
    var likedNote = $("div[data-note-id=" + id + "]");
    var btn = likedNote.find("button[data-liked]");
    var span = btn.find("span.like-heart");
    btn.data("liked", true);
    span.removeClass("glyphicon-heart-empty");
    span.addClass("glyphicon-heart");
                }
            }

        }).fail(function () { });
    //beğenilmemiş bir gönderi olduğunda ve biz beğeni butonuna tıkladığımızda 1- veritabanında likes tablosuna ilgili kayıt girilmeli.
    //beğenilen bir gönderi için aynı butona tıklandığında da ilgili kayıt veritabanından silinmeli..
    //sayfa yüklendikten sonra data-liked attribute'ü olan butonlardan hangisine tıklandıysa click(). aşağıdaki metot bunun için çalışacak.
    $("button[data-liked]").click(function () {
            //data-liked true ise gönderi beğenilmiş. false ise gönderi beğenilmemiştir.
            //önce butonu buluyorum.
            var btn = $(this); // o anki butonu btn değişkenine atıyorum..
    var liked = btn.data("liked"); // true ya da false değerini alıyorum.
    var noteid = btn.data("note-id"); //  gönderi beğenildiyse beğenilmedi yapılacak. beğenilmediyse beğenildi yapılacak. bu amaç için kullanacağım.
    var spanHeart = btn.find("span.like-heart");
    var spanCount = btn.find("span.like-count");
    //gerekli bilgileri sayfadan toparladım.
    $.ajax({
        method: "POST",
    url: "/Note/SetNoteLike",
    data: {"noteid": noteid, "liked": !liked } //noteid'yi ve liked'ın olması gereken değerini action'a gönderiyorum.
            }).done(function (data) {   //action'dan gelen sonucu data ile alıyorum.
                if (data.hasError) {    //datanın içindeki HasError'e göre; true ise hata var kullanıcı uyarılıyor. 
        alert(data.errorMessage);
                } else {        // false ise sayfadaki liked değerini değiştiriyorum.
        liked = !liked
                    btn.data("liked", liked);
    spanCount.text(data.result);    // action'dan gelen beğeni sayısını ilgili yere yazdırıyorum.
    //beğeni butonundaki icon'ları/class'ları kaldırıyorum.
    spanHeart.removeClass("glyphicon-heart-empty");
    spanHeart.removeClass("glyphicon-heart");
    // aşağıdaki if bloğunda beğenilme ya da beğenilmeyi kadlırma işlemine göre ikonu değiştiriyorum.
    if (liked) {
        spanHeart.addClass("glyphicon-heart");
                    }
    else {
        spanHeart.addClass("glyphicon-heart-empty");
                    }
                }
            }).fail(function () {
        //giriş yapılmadığında fakat beğeni yapıldığında aşağıdaki uyarıyı kullanıcıya gösteriyorum.
        alert("Gönderiyi beğenmek için sisteme giriş yapmalısınız.");
            });
        })
    });
