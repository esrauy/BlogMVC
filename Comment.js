
    modelComponentBodyId = '#modal_yorum_body';
    var noteId = -1;
    $(function () {
        $('#modal_yorum').on('show.bs.modal', function (e) {
            var btn = $(e.relatedTarget);
            noteId = btn.data("note-id");
            $('#modal_yorum_body').load('/Comment/ShowNoteComments/' + noteId)
        })
    });

    function doComment(btn, eventMode, commentId, spanId)
    {
        var button = $(btn); //parametreden gelen btn nesnesini JQuery'de kullanıma hazırladım.
    var mode = button.data("edit-mode");  //partial'daki button içindeki data-edit-mode özelliğinin değerini alıyorum.
    if (eventMode === "edit-clicked")
    {
            if (!mode) {            //mode false ise aşağıdaki işlemleri yapacak.
        button.data("edit-mode", true);    //data-edit-mode=true yapıyorum. burası güncellenebilir bilgisini ttmak için kullanılıyor.
    button.removeClass("btn-warning");     //button'un class'ında olan btn-warning class'ını kaldırıyorum.
    button.addClass("btn-success");     //button'un class'ına btn-success class'ını ekliyorum.

    var btnSpan = button.find("span");      //button'un içindeki span'i bulup btnSpan isimli değişkene aktarıyorum.
    btnSpan.removeClass("glyphicon-edit");  //Span'in içindeki edit class'ını kaldırıyorum.
    btnSpan.addClass("glyphicon-ok");       //span'in class'ına glyphicon-ok class'ını ekliyorum.

    // doComment fonksiyonuna parametre olarak gönderilen spanId'si yani partial içindeki yorumun yazıldığı span... comment.text'in yazıldığı satıra ait olan span..
    $(spanId).attr("contenteditable", true); //ilgili span'in contenteditable özelliğine true değerini aktarıyorum. böylece bu span edit edilebilir textbox'a dönüşecek.
    $(spanId).addClass("editable"); // ilgili span'a editable isminde bir class ekliyorum. css ekleyip bir takım özelliklerini değiştireceğim.
    $(spanId).focus();  //span edit edilebilir hale geldikten sonra yani textbox olduktan sonra cursor burada konumlansın diye yazdığım satır.
            }
    else      //mode true olduğunda else kısmı çalışacak ve yukarıdaki işlemlerin tam tersini yapacak.
    {
        button.data("edit-mode", false);
    button.removeClass("btn-success");
    button.addClass("btn-warning");

    var btnSpan = button.find("span");
    btnSpan.removeClass("glyphicon-ok");
    btnSpan.addClass("glyphicon-edit");

    $(spanId).attr("contenteditable", false);
    $(spanId).removeClass("editable");

    //burada edit edilen yorumu veritabanına kaydetmemiz gereken kodları yazacağız. değişikliği ilgili controller ve action'a göndereceğiz.

    var txt = $(spanId).text(); //ilgili span içinden text değerini yani değiştirilmiş yorumu alıyoruz.
    $.ajax({
        method: "POST",
    url: "/Comment/Edit/" + commentId,
    data: {text: txt }
                }).done(function (data) {   //ajax metodu sonucu başarılı olursa burası yani done kısmı çalışacak.
                    if (data.result) {
        $(modelComponentBodyId).load('/Comment/ShowNoteComments/' + noteId);
                    } else {
        //güncelleme yapılamamış oluyor. burada da kullanıcıya mesaj vereceğiz.
        alert("Yorum güncellenemedi.");
                    }
                }).fail(function () {   //ajax metodu sonucu başarısız olursa burası yani fail kısmı çalışacak.
        //Action ile ilgili bir problem olduğunda çalışacak bölüm.
        alert("Sunnucuya bağlanılamadı.");
                });

            }                      
        }
    else if (eventMode === 'delete-clicked') {
            // Kullanıcı silme butonuna bastığında bir onay almamız gerekecek.
            var dialogResult = confirm("Yorum silinsin mi?");
    if (!dialogResult) return false;        // silme işlemi iptal edilecek.
    $.ajax({
        method: "GET",
    url: "/Comment/Delete/" + commentId
            }).done(function (data) {   // ajax metodu sonucu başarılı olursa burası yani done kısmı çalışacak
                //data ile action'dan bir değer gelecek true ya da false.. true: silme işlemi başarılı. false: silme işlemi başarısız
                if (data) {
        //silme işlemi başarılı ise yorumlar sayfasını güncellemesi gerekiyor. yani silinen yorum ekrana gelmeyecek.
        $(modelComponentBodyId).load('/Comment/ShowNoteComments/' + noteId); //yorumlar bölümünü tekrardan yükleyecek.
                }
    else {
        alert("Yorum silinemedi.");
                }

            }).fail(function () {   // ajax metodu sonucu başarısız olursa burası yani fail kısmı çalışacak
        // Action ile ilgili bir problem olduğunda çalışacak bölüm.
        alert("Sunucuya bağlanamadı. Silme işlemi iptal edildi.");
            });
        }
    else if (eventMode === 'new-clicked') {
            //eklenen yorumu iilgili input'un text'inden alacağım ve veritabanına kaydolsun diye Insert Action'ına göndermem gerekecek.
            var txt = $('#new_comment_text').val(); //input içindeki veriyi almak için yazdığımız kod
    //alert("Girilen yorum: " + txt)
    $.ajax({
        method: "POST",
    url: "/Comment/Create/",
    data: {"Text": txt, "noteId": noteId } //"Text" şeklinde çift tırnak içinde yazdığımda değişken adı olarak algılanacak.
            }).done(function (data) {   // ajax metodu sonucu başarılı olursa burası yani done kısmı çalışacak
                //data ile action'dan bir değer gelecek true ya da false.. true: silme işlemi başarılı. false: silme işlemi başarısız
                if (data) {
        // _partialComment ile verileri tekrardan yüklüyoruz.
        $(modelComponentBodyId).load('/Comment/ShowNoteComments/' + noteId); //yorumlar bölümünü tekrardan yükleyecek.
                }
    else {
        alert("Yorum eklenemedi.");
                }

            }).fail(function () {   // ajax metodu sonucu başarısız olursa burası yani fail kısmı çalışacak
        // Action ile ilgili bir problem olduğunda çalışacak bölüm.
        alert("Sunucuya bağlanamadı. Yorum ekleme işlemi iptal edildi.");
            });
        }
    }

