"use strict";

var comments = (function () {

    var like = function (button) {
        var $likeIcon = $(button).find("i");
        if ($likeIcon.hasClass("mdi-heart-outline")) {
            $likeIcon.removeClass("mdi-heart-outline").addClass("mdi-heart");            
        } else {
            $likeIcon.removeClass("mdi-heart").addClass("mdi-heart-outline");
        }       
        return false;
    };

    var addComment = function (postId) {

        var $postSection = $("#post-" + postId); 
        $postSection.find(".js-error-message").hide();
        var $commentInput = $postSection.find("input");
        if ($commentInput.val() === "") {
            $postSection.find(".js-error-message").show();
            return;
        }
              
        var $commentContainer = $postSection.find(".js-comments-container");

        var templateLiteral = `<div class="border border-light p-2 mb-3">
                                        <div class="media">
                                            <div class="media-body">
                                                <h5 class="m-0">Anonymous</h5>
                                            </div>
                                        </div>
                                        <p class="comment">${$commentInput.val()}</p>
                                    </div>`;

        $commentContainer.append(templateLiteral);
        $commentInput.val("");
    };

    var init = function () {
        
    };

    init();

    return {
        like: like,
        addComment: addComment
    };
})();

