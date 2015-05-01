/*
    Bulk import sink expects stored proc to return an array of objects in the following form:

    {
        i: <index of the document in the batch>,
        e: <error message, if any>
    }

    "i" should match what was provided in the input "items" array and will be used
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
        return;
    }

    var options = { disableAutomaticIdGeneration: disableAutomaticIdGeneration != 0 };

    tryCreate(items[itemsState.length], callback);

    function tryCreate(item, callback) {
        try {
            if (!collection.createDocument(collectionLink, item.d, options,
                    function (error, doc, options) { callback(item.i, error); })) {
                // If request was not accepted, return the results of bulk operation right away
                getContext().getResponse().setBody(itemsState);
            }
        } catch (error) {
            callback(item.i, error);
        }
    }

    function callback(docIndex, error) {
        var itemState = { i: docIndex };
        if (error) {
            itemState.e = error.message || "Failed to create document";
        }

        itemsState.push(itemState);

        if (itemsState.length < items.length) {
            tryCreate(items[itemsState.length], callback);
        } else {
            getContext().getResponse().setBody(itemsState);
        }
    }
}
