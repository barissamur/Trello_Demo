$(function () {
    fetchCardLists();

    function fetchCardLists() {
        $.ajax({
            url: '/api/Data/GetCardList',
            method: 'GET',
            success: function (cardLists) {
                displayCardLists(cardLists);
            },
            error: function (e) {
                console.log("Kart listeleri yüklenirken bir hata oluştu.");
            }
        });
    }

    function displayCardLists(cardLists) {
        cardLists.forEach(list => {
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
        makeCardListsSortable();

    }

    function makeCardsSortable() {
        $('.sortable-cards').sortable({
            connectWith: '.sortable-cards',
            placeholder: 'card-placeholder',
            start: function (event, ui) {
                ui.item.data('start-pos', ui.item.index());
                ui.item.data('start-list', ui.item.closest('.card-list').data('list-id'));
            },
            stop: function (event, ui) {
                var startListId = ui.item.data('start-list');
                var endListId = ui.item.closest('.card-list').data('list-id');
                var startPos = ui.item.data('start-pos');
                var endPos = ui.item.index();


                var affectedLists = [startListId];
                if (startListId != endListId) {
                    affectedLists.push(endListId);
                }

                if (!(startListId == endListId && startPos == endPos)) {
                    updateAffectedLists(affectedLists);
                }
            }
        });
    }

    function updateAffectedLists(affectedListIds) {
        var data = [];

        affectedListIds.forEach(function (listId) {
            var list = $('[data-list-id="' + listId + '"]');
            var listIndex = list.index();

            var cardsNewIndex = {};
            list.find('.card').each(function (index) {
                var cardId = $(this).data('card-id');
                cardsNewIndex[cardId] = index;
            });

            data.push({
                ListId: parseInt(listId),
                ListIndex: listIndex,
                CarddNewIndex: cardsNewIndex
            });
        });

        setIndexCards(data);
    }

    function setIndexCards(data) {
        $.ajax({
            url: '/api/Data/SetIndexCards',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                console.log("Liste indeksleri başarıyla güncellendi.");
            },
            error: function (xhr, status, error) {
                console.error("Güncelleme sırasında bir hata oluştu: ", error);
            }
        });
    }


    // cardlist işlemleri
    function makeCardListsSortable() {
        $('#sortable-container').sortable({
            items: '> .card-list', // Sadece doğrudan çocuk olan card-list sınıfına sahip elemanları sıralanabilir yapar
            placeholder: 'list-placeholder', // Sıralama sırasında gösterilecek placeholder
            start: function (event, ui) {

            },
            stop: function (event, ui) {
                updateAllListIndexes();
            }
        });
    }
    function updateAllListIndexes() {
        var data = $('#sortable-container .card-list').map(function (index) {
            return {
                ListId: $(this).data('list-id'),
                ListIndex: index
            };
        }).get();

        console.log(data);
        setIndexCardLists(data);
    }

    function setIndexCardLists(data) {
        $.ajax({
            url: '/api/Data/SetIndexCardLists',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                console.log("Kart indeksleri başarıyla güncellendi.");
            },
            error: function (xhr, status, error) {
                console.error("Güncelleme sırasında bir hata oluştu: ", error);
            }
        });
    }


});
