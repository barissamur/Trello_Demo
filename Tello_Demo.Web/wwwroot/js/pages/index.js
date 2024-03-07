$(function () {
    let affectedCardId;
    let affectedListId;
    let eventName;
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
                    <button type="button" class="delete-card btn btn-sm" data-card-id="${card.id}">&#10006;</button>
                 </div>`;
            });

            listHtml += `   </div>
                        <button type="button" class="add-card btn btn-secondary" data-list-id="${list.id}">+</button>
                      </div>`;


            $('#sortable-container').append(listHtml);
        });

        $('#sortable-container').append(`<div id="add-card-list" class="add-card-list">
                                            <span>+</span>
                                         </div>`);

        $('.card-list h3').on('click', function () {
            editCardListTitle($(this));
        });

        $('#add-card-list').on('click', function () {
            Swal.fire({
                title: 'Liste için bir isim giriniz',
                input: 'text',
                inputPlaceholder: 'Liste adı',
                showCancelButton: true,
                confirmButtonText: 'Oluştur',
                cancelButtonText: 'İptal',
                inputValidator: (value) => {
                    if (!value) {
                        return 'Liste adı girmelisiniz!';
                    }
                }
            }).then((result) => {
                if (result.value) {
                    let listName = result.value;
                    createNewList(listName);
                }
            });
        });


        $('.delete-list').on('click', function () {
            var listId = $(this).data('list-id');

            Swal.fire({
                title: 'Emin misiniz?',
                text: "Bu listeyi silmek istediğinize emin misiniz?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, sil!',
                cancelButtonText: 'İptal'
            }).then((result) => {
                if (result.isConfirmed) {
                    deleteList(listId);
                }
            });
        });


        $('.delete-card').on('click', function (event) {
            event.stopPropagation();

            var cardId = $(this).data('card-id');
            let card = $(this).closest('.card');
            let cardTitle = card.find('p').text();
            console.log(card);
            Swal.fire({
                title: 'Emin misiniz?',
                text: "Bu kartı silmek istediğinize emin misiniz?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, sil!',
                cancelButtonText: 'İptal'
            }).then((result) => {
                if (result.isConfirmed) {
                    deleteCard(cardId, cardTitle);
                }
            });
        });

        $('.add-card').on('click', function () {
            let listId = $(this).data('list-id');
            addNewCardToList(listId);
        });

        $('.card').on('click', function () {
            let cardId = $(this).data('card-id');
            let cardTitle = $(this).find('p').text();
            let cardIndex = $(this).index();

            let list = $(this).closest('.card-list');
            let listId = list.data('list-id');
            let listTitle = list.find('h3').text();
            let listIndex = list.index();

            $('#card-title').val(cardTitle);
            $('#cardEditModal').data('card-id', cardId)
                .data('list-id', listId)
                .data('list-title', listTitle)
                .data('card-index', cardIndex)
                .data('list-index', listIndex);

            $('#cardEditModal').on('shown.bs.modal', function () {
                fetchLogData(cardId);
            });
            // Modalı aç
            $('#cardEditModal').modal('show');
        });

        makeCardsSortable();
        makeCardListsSortable();
    }


    function fetchLogData() {
        $.ajax({
            url: '/api/Data/GetLogData', 
            method: 'GET',
            dataType: 'json', 
            success: function (cardLogs) {
                var logsHtml = cardLogs.map(function (log) {
                    var eventTime = new Date(log.eventTime).toLocaleString();
                    return `<div class="log-entry">
                            <div class="log-date">${eventTime}</div>
                            <div class="log-details">${log.details}</div>
                        </div>`;
                }).join('');
                $('#card-logs').html(logsHtml); 
            },
            error: function (xhr, status, error) {
                console.error("Log verisi alınırken bir hata oluştu:", error);
            }
        });
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
                affectedCardId = ui.item.data('card-id');
                var startListId = ui.item.data('start-list');
                var endListId = ui.item.closest('.card-list').data('list-id');
                var startPos = ui.item.data('start-pos');
                var endPos = ui.item.index();

                var affectedLists = [startListId];

                if (startListId != endListId)
                    affectedLists.push(endListId);

                if (!(startListId == endListId && startPos == endPos))
                    updateAffectedLists(affectedLists);

            }
        });
    }

    function updateAffectedLists(affectedListIds) {
        eventName = affectedListIds.length == 2 ? "Taşındı" : "Güncellendi";

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
            url: `/api/Data/UpdateCardListAndCards?cardId=${affectedCardId}&eventName=${eventName}`,
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


    // cardlist işlemleri
    function makeCardListsSortable() {
        $('#sortable-container').sortable({
            items: '> .card-list',
            placeholder: 'list-placeholder',
            start: function (event, ui) {

            },
            stop: function (event, ui) {
                affectedListId = ui.item.data('list-id');
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

        setIndexCardLists(data);
    }

    function setIndexCardLists(data) {
        eventName = 'Güncellendi';
        $.ajax({
            url: `/api/Data/UpdateCardListAndCards?listId=${affectedListId}&eventName=${eventName}`,
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

    // click işlemleri 
    function editCardListTitle(headerElement) {
        eventName = 'İsim güncellendi!';

        var currentTitle = headerElement.text();
        var listId = headerElement.closest('.card-list').data('list-id');
        var newTitle = prompt("Yeni başlık giriniz:", currentTitle);

        if (newTitle && newTitle !== currentTitle) {
            headerElement.text(newTitle);

            var dataToSend = [
                {
                    Id: listId,
                    Title: newTitle,
                    Type: 'Type',
                    Cards: []
                }
            ];

            $.ajax({
                url: `/api/Data/UpdateCardListAndCards?listId=${listId}&eventName=${eventName}`,
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(dataToSend),
                success: function () {
                    console.log("Liste başlığı başarıyla güncellendi.");
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
                console.log("Liste başarıyla eklendi.");
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

    function deleteCard(cardId, cardTitle) {
        $.ajax({
            url: `/api/Data/DeleteCard/${cardId}?cardTitle=${cardTitle}`,
            method: 'DELETE',
            success: function () {
                console.log("Card başarıyla silindi.");
                fetchCardLists();
            },
            error: function (xhr, status, error) {
                console.log(xhr, status);
                console.error("Card silinirken bir hata oluştu: ", error);
            }
        });
    }

    function addNewCardToList(listId) {
        Swal.fire({
            title: 'Yeni kart için bir başlık girin',
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            confirmButtonText: 'Ekle',
            cancelButtonText: 'İptal',
            showLoaderOnConfirm: true,
            preConfirm: (cardTitle) => {
                if (!cardTitle) {
                    Swal.showValidationMessage("Başlık girmelisiniz");
                    return;
                }

                var list = $('[data-list-id="' + listId + '"]');
                var listTitle = list.find('h3').text();

                let data = {
                    title: cardTitle,
                    cardList: {
                        title: listTitle
                    }
                };

                return $.ajax({
                    url: '/api/Data/AddCardToList/' + listId,
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(data),
                    success: function () {
                        console.log("Yeni kart başarıyla eklendi.");
                        fetchCardLists();
                    },
                    error: function (xhr, status, error) {
                        Swal.showValidationMessage(`İstek hatası: ${error}`);
                    }
                });
            },
            allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    showConfirmButton: false,
                    timer: 1500,
                    title: `Yeni kart başarıyla eklendi!`
                });
            }
        });
    }


    $('#save-card-changes').off('click').on('click', function () {
        eventName = 'İsim güncellendi!';
        let cardId = $('#cardEditModal').data('card-id');
        let newTitle = $('#card-title').val();

        let listId = $('#cardEditModal').data('list-id');
        let listTitle = $('#cardEditModal').data('list-title');
        let cardIndex = $('#cardEditModal').data('card-index');
        let listIndex = $('#cardEditModal').data('list-index');

        let data = [{
            Id: listId,
            Title: listTitle,
            Index: listIndex,
            Type: 'Type',
            Cards: [{
                Id: cardId,
                Title: newTitle,
                Index: cardIndex,
                Description: 'Açıklama',
                Type: 'Type'
            }]
        }];

        $.ajax({
            url: `/api/Data/UpdateCardListAndCards?cardId=${cardId}&eventName=${eventName}`,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                console.log("Kart başarıyla güncellendi.");
                $('[data-card-id="' + cardId + '"]').find('p').text(newTitle);
            },
            error: function (xhr, status, error) {
                console.error("Güncelleme sırasında bir hata oluştu: ", error);
            }
        });

        // Modalı kapat
        $('#cardEditModal').modal('hide').removeData('card-id').removeData('list-id').removeData('list-title');
    });
});
