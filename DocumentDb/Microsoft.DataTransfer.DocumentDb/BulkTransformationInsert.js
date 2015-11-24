/*
    Bulk import sink expects stored proc to return an array of objects in the following form:

    {
        i: <index of the document in the batch>,
        e: <error message, if any>
    }

    "i" should match what was provided in the input "items" array and will be used
    to associate error messages with each specific document.

    This version of the stored procedure allows to perform custom document transformation.
    Look for transformDocument(...) function for additional information.
*/
function BulkImport(items, updateExisting, disableAutomaticIdGeneration) {
    var collection = getContext().getCollection();
    var collectionLink = collection.getSelfLink();

    var createDocumentFunction = updateExisting != 0 ? collection.upsertDocument : collection.createDocument;

    var itemsState = [];

    if (!items) {
        throw new Error("The items array is undefined or null.");
    }

    if (items.length == 0) {
        getContext().getResponse().setBody(itemsState);
        return;
    }

    function transformDocument(doc) {
        /*
            Add custom tranformation code here. You can either modify existing document and
            return it or create a new one.

            Example #1, rename "ExternalId" field to "id":

                doc["id"] = doc["ExternalId"];
                delete doc.ExternalId;

            Example #2, convert string field to an array:

                if (doc["Cities"]) {
                    doc["Cities"] = doc["Cities"].split("|");
                }
        */
        return doc;
    }

    var options = { disableAutomaticIdGeneration: disableAutomaticIdGeneration != 0 };

    tryCreate(items[itemsState.length], callback);

    function tryCreate(item, callback) {
        try {
            if (!createDocumentFunction(collectionLink, transformDocument(item.d), options,
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
