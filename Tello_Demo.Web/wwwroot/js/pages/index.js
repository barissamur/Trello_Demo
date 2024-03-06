$(function () {
    fetchCardLists();

    function fetchCardLists() {
        $.ajax({
            url: '/api/Data/GetCardList',
            method: 'GET',
            success: function (cardLists) {
                displayCardLists(cardLists);
            },
            error: function () {
                console.log("Kart listeleri yüklenirken bir hata oluştu.");
            }
        });
    }

    function displayCardLists(cardLists) {

        $('#sortable-container').empty();

        cardLists.forEach(list => {
            let listHtml = `<div class="card-list" data-list-id="${list.id}">
                                <div class="d-flex justify-content-between p-2 bg-primary">
                                    <h3>${list.title}</h3>
                                    <button type="button" class="delete-list btn" data-list-id="${list.id}">&#10006;</button>
                                </div>
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

        $('#sortable-container').append(`<div id="add-card-list" class="add-card-list">
                                            <span>+</span>
                                         </div>`);

        $('.card-list h3').on('click', function () {
            editTitle($(this));
        });

        $('#add-card-list').on('click', function () {
            var listName = prompt("Liste için bir isim giriniz:");
            if (listName) {
                createNewList(listName);
            }
        });

        $('.delete-list').on('click', function () {
            var listId = $(this).data('list-id');
            if (confirm("Bu listeyi silmek istediğinize emin misiniz?")) {
                deleteList(listId);
            }
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
        var data = affectedListIds.map(function (listId) {
            var list = $('[data-list-id="' + listId + '"]');
            var listIndex = list.index();
            var listTitle = list.find('h3').text();

            var cards = list.find('.card').map(function (index) {
                return {
                    Id: $(this).data('card-id'),
                    Index: index,
                    Title: $(this).find('p').text(),
                    Description: 'Açıklama',
                    Type: 'Type'

                };
            }).get();

            return {
                Id: listId,
                Index: listIndex,
                Title: listTitle,
                Cards: cards,
                Type: 'Type'

            };
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
                fetchCardLists();
            },
            error: function (xhr, status, error) {
                console.error("Güncelleme sırasında bir hata oluştu: ", error);
            }
        });
    }


    // cardlist işlemleri
    function makeCardListsSortable() {
        $('#sortable-container').sortable({
            items: '> .card-list',
            placeholder: 'list-placeholder',
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
                Id: $(this).data('list-id'),
                Index: index,
                Title: $(this).find('h3').text(),
                Type: 'Type'
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

    // click işlemleri 
    function editTitle(headerElement) {
        var currentTitle = headerElement.text();
        var listId = headerElement.closest('.card-list').data('list-id');
        var newTitle = prompt("Yeni başlık giriniz:", currentTitle);

        if (newTitle && newTitle !== currentTitle) {
            headerElement.text(newTitle);

            $.ajax({
                url: '/api/Data/UpdateListTitle',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ listId: listId, title: newTitle }),
                success: function () {
                    console.log("Liste başlığı başarıyla güncellendi.");
                    fetchCardLists();
                },
                error: function (xhr, status, error) {
                    console.error("Başlık güncellenirken bir hata oluştu: ", error);
                    headerElement.text(currentTitle);
                }
            });
        }
    }

    function createNewList(listName) {
        let data = {
            Title: listName,
            Index: 999,
            Type: 'Yeni'
        };


        $.ajax({
            url: '/api/Data/CreateList',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                fetchCardLists();
            },
            error: function (xhr, status, error) {
                console.error("Yeni liste eklenirken bir hata oluştu:", error);
            }
        });

    }

    function deleteList(listId) {
        $.ajax({
            url: '/api/Data/DeleteList/' + listId,
            method: 'DELETE',
            success: function () {
                console.log("Liste başarıyla silindi.");
                fetchCardLists();
            },
            error: function (xhr, status, error) {
                console.log(xhr, status);
                console.error("Liste silinirken bir hata oluştu: ", error);
            }
        });
    }

});
