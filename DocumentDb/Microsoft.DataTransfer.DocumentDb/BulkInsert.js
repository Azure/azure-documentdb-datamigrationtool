/*
    Bulk import sink expects stored proc to return an array of objects in the following form:

    {
        DocumentIndex: <index of the document in the batch>,
        ErrorMessage: <error message, if any>
    }

    DocumentIndex should match what was provided in the input "items" array and will be used
    to associate error messages with each specific document.
*/
function BulkImport(items, disableAutomaticIdGeneration) {
    var collection = getContext().getCollection();
    var collectionLink = collection.getSelfLink();

    var itemsState = [];

    if (!items) {
        throw new Error("The items array is undefined or null.");
    }

    if (items.length == 0) {
        getContext().getResponse().setBody(itemsState);
    }

    var options = { disableAutomaticIdGeneration: disableAutomaticIdGeneration };

    tryCreate(items[itemsState.length], callback);

    function tryCreate(item, callback) {
        if (!collection.createDocument(collectionLink, item.Document, options,
                function (error, doc, options) { callback(error, item.DocumentIndex, doc, options); })) {
            // If request was not accepted, return the results of bulk operation right away
            getContext().getResponse().setBody(itemsState);
        }
    }

    function callback(error, docIndex, doc, options) {
        var itemState = { DocumentIndex: docIndex };
        if (error) {
            itemState.ErrorMessage = error.message || "Failed to create document";
        }

        itemsState.push(itemState);

        if (itemsState.length < items.length) {
            tryCreate(items[itemsState.length], callback);
        } else {
            getContext().getResponse().setBody(itemsState);
        }
    }
}
