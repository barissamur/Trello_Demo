$(function () {
    fetchCardLists();

    function fetchCardLists() {
        $.ajax({
            url: '/api/Data/GetCardList', // Önceden oluşturduğunuz endpoint
            method: 'GET',
            success: function (cardLists) {
                displayCardLists(cardLists);
            },
            error: function (e) {
                console.log(e);
                //alert("Kart listeleri yüklenirken bir hata oluştu.");
            }
        });
    }

    function displayCardLists(cardLists) {
        cardLists.forEach(list => {
            // Her bir kart listesi için HTML yapısını oluştur
            let listHtml = `<div class="card-list" data-list-id="${list.id}">
                                <h3>${list.title}</h3>
                                <div class="sortable-cards">`;

            list.cards.forEach(card => {
                listHtml += `<div class="card" data-card-id="${card.id}">
                                <p>${card.title}</p>
                             </div>`;
            });

            listHtml += `   </div>
                          </div>`;

            $('#sortable-container').append(listHtml);
        });

        makeCardsSortable();
    }

    function makeCardsSortable() {
        $('.sortable-cards').sortable({
            connectWith: '.sortable-cards',
            placeholder: 'card-placeholder',
            start: function (event, ui) {
                // Sürüklemeye başlanıldığında kaynak listeyi kaydedin
                ui.item.data('start-pos', ui.item.index());
                ui.item.data('start-list', ui.item.closest('.card-list').data('list-id'));
            },
            stop: function (event, ui) {
                // Sürükleme durduğunda hedef listeyi alın
                console.log(ui.item);
                var startPos = ui.item.data('start-pos');
                var startListId = ui.item.data('start-list');
                var endListId = ui.item.closest('.card-list').data('list-id');
                var endPos = ui.item.index();
                var cardId = ui.item.data('card-id');

                getCardListWithCards(startListId);

                if (startListId != endListId) {
                    getCardListWithCards(endListId);
                }
            }
        });
    }

    function getCardListWithCards(listId) {
        var cards = $('[data-list-id="' + listId + '"] .card');
        let data = cards.map(function (index, card) {
            return {
                cardId: $(card).data('card-id'),
                listId: listId,
                newIndex: index
            };
        }).get();



        setIndexCards();
    }


    function setIndexCards() {
        var data = prepareSetIndexData();

        $.ajax({
            url: '/api/Data/SetIndex',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                console.log("Liste ve kart indeksleri başarıyla güncellendi.");
            },
            error: function (xhr, status, error) {
                console.error("Güncelleme sırasında bir hata oluştu: ", error);
            }
        });
    }


    function prepareSetIndexData() {
        var setIndexData = [];

        $('.card-list').each(function () {
            var listId = $(this).data('list-id');
            var listIndex = $(this).index();

            var cardsNewIndex = {};
            $(this).find('.card').each(function (index) {
                var cardId = $(this).data('card-id');
                cardsNewIndex[cardId] = index;
            });

            setIndexData.push({
                ListId: listId,
                ListIndex: listIndex,
                CarddNewIndex: cardsNewIndex
            });
        });

        return setIndexData;
    }

});
